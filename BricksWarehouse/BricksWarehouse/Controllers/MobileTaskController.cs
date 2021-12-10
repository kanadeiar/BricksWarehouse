using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BricksWarehouse.Controllers;

[Route("api/[controller]"), ApiController]
public class MobileTaskController : ControllerBase
{
    private readonly IOutTaskData _outTaskData;
    private readonly IProductTypeData _productTypeData;
    private readonly IPlaceData _placeData;
    private readonly IMapper<OutTask, OutTaskDto> _mapperTo;
    private readonly IMapper<ProductType, ProductTypeDto> _mapperProductTypeTo;
    private readonly IMapper<Place, PlaceDto> _mapperPlaceTo;

    public MobileTaskController(IOutTaskData outTaskData, IProductTypeData productTypeData, IPlaceData placeData, IMapper<OutTask, OutTaskDto> mapperTo, 
        IMapper<ProductType, ProductTypeDto> mapperProductTypeTo, IMapper<Place, PlaceDto> mapperPlaceTo)
    {
        _outTaskData = outTaskData;
        _productTypeData = productTypeData;
        _placeData = placeData;
        _mapperTo = mapperTo;
        _mapperProductTypeTo = mapperProductTypeTo;
        _mapperPlaceTo = mapperPlaceTo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = (await _outTaskData.GetAllAsync(true)).Where(ot => !ot.IsCompleted).OrderBy(ot => ot.CreatedDateTime);
        return Ok(tasks.Select(t => _mapperTo.Map(t)));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _outTaskData.GetAsync(id);
        if (task is null)
            return NotFound();
        return Ok(_mapperTo.Map(task));
    }

    [HttpGet("producttype/{format:int}")]
    public async Task<IActionResult> FindProductTypeByFormat(int format)
    {
        var productType = await _productTypeData.GetByFormatAsync(format);
        if (productType is null)
            return NotFound();
        return Ok(_mapperProductTypeTo.Map(productType));
    }

    [HttpGet("producttypeplaces/{productTypeId:int}")]
    public async Task<IActionResult> GetRecommendedLoadPlaces(int productTypeId)
    {
        var places = (await _placeData.GetAllAsync())
            .Where(p => p.ProductTypeId == null || (p.ProductTypeId == productTypeId && p.Count < p.Size) )
            .OrderByDescending(p => p.Count);
        return Ok(places.Select(p => _mapperPlaceTo.Map(p)));
    }

    [HttpGet("place/{number:int}")]
    public async Task<IActionResult> FindPlaceByNumber(int number)
    {
        var place = await _placeData.GetByNumberAsync(number);
        if (place is null)
            return NotFound();
        return Ok(_mapperPlaceTo.Map(place));
    }

    [HttpGet("load/{productTypeId:int}/{placeId:int}/{count:int}")]
    public async Task<IActionResult> LoadProductToPlace(int productTypeId, int placeId, int count)
    {
        var place = await _placeData.GetAsync(placeId);
        if (place is null)
            return NotFound();
        if (place.ProductTypeId is null)
            place.ProductTypeId = productTypeId;
        if (place.ProductTypeId == productTypeId)
        {
            place.Count += count;
            place.LastDateTime = DateTime.Now;
            return Ok(_mapperPlaceTo.Map(place));
        }
        return NotFound();
    }

    [HttpGet("producttypeshipment/{productTypeId:int}")]
    public async Task<IActionResult> GetRecommendedShipmentPlaces(int productTypeId)
    {
        var places = (await _placeData.GetAllAsync())
            .Where(p => p.ProductTypeId == productTypeId && p.Count > 0)
            .OrderBy(p => p.LastDateTime);
        return Ok(places.Select(p => _mapperPlaceTo.Map(p)));
    }

    [HttpGet("shipment/{placeId:int}/{taskId:int}/{count:int}")]
    public async Task<IActionResult> ShipmentProductFromPlace(int placeId, int taskId, int count)
    {
        var task = await _outTaskData.GetAsync(taskId);
        var place = await _placeData.GetAsync(placeId);
        if (task is null || place is null || task.ProductTypeId is null || place.ProductTypeId is null)
            return NotFound();
        if (count > place.Count || count > task.Count - task.Loaded)
            return NotFound();
        if (task.ProductTypeId == place.ProductTypeId)
        {
            place.Count -= count;
            if (place.Count <= 0)
            {
                place.ProductTypeId = null;
            }
            await _placeData.UpdateAsync(place);
            task.Loaded += count;
            if (task.Loaded >= task.Count)
                task.IsCompleted = true;
            await _outTaskData.UpdateAsync(task);
            return Ok(_mapperPlaceTo.Map(place));
        }
        return NotFound();
    }
}

