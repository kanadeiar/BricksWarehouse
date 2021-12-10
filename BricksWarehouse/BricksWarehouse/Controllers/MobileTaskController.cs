using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BricksWarehouse.Controllers;

[Route("api/[controller]"), ApiController]
public class MobileTaskController : ControllerBase
{
    private readonly GetTaskDataService _getTaskDataService;
    private readonly IOutTaskData _outTaskData;
    private readonly IProductTypeData _productTypeData;
    private readonly IPlaceData _placeData;
    private readonly IMapper<OutTask, OutTaskDto> _mapperTo;
    private readonly IMapper<ProductType, ProductTypeDto> _mapperProductTypeTo;
    private readonly IMapper<Place, PlaceDto> _mapperPlaceTo;

    public MobileTaskController(GetTaskDataService getTaskDataService, IOutTaskData outTaskData, IProductTypeData productTypeData, IPlaceData placeData, IMapper<OutTask, OutTaskDto> mapperTo, 
        IMapper<ProductType, ProductTypeDto> mapperProductTypeTo, IMapper<Place, PlaceDto> mapperPlaceTo)
    {
        _getTaskDataService = getTaskDataService;
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

    [HttpGet("producttypesload/{productTypeId:int}")]
    public async Task<IActionResult> GetRecommendedLoadPlaces(int productTypeId)
    {
        var places = await _getTaskDataService.GetRecommendedLoadPlaces(productTypeId);
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
        var place = await _getTaskDataService.LoadProductToPlace(productTypeId, placeId, count);
        if (place is not null)
        {
            return Ok(_mapperPlaceTo.Map(place));
        }
        return NotFound();
    }

    [HttpGet("producttypeshipment/{productTypeId:int}")]
    public async Task<IActionResult> GetRecommendedShipmentPlaces(int productTypeId)
    {
        var places = await _getTaskDataService.GetRecommendedShipmentPlaces(productTypeId);
        return Ok(places.Select(p => _mapperPlaceTo.Map(p)));
    }

    [HttpGet("shipment/{placeId:int}/{taskId:int}/{count:int}")]
    public async Task<IActionResult> ShipmentProductFromPlace(int placeId, int taskId, int count)
    {
        var place = await _getTaskDataService.ShipmentProductFromPlace(placeId, taskId, count);
        if (place is not null)
        {
            return Ok(_mapperPlaceTo.Map(place));
        }
        return NotFound();
    }
}

