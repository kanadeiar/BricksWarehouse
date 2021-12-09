using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BricksWarehouse.Controllers;

[Route("api/[controller]"), ApiController]
public class MobileTaskController : ControllerBase
{
    private readonly WarehouseContext _context;
    private readonly IOutTaskData _outTaskData;
    private readonly IMapper<OutTask, OutTaskDto> _mapperTo;
    private readonly IMapper<OutTaskDto, OutTask> _mapperFrom;
    private readonly IMapper<ProductType, ProductTypeDto> _mapperProductTypeTo;
    private readonly IMapper<Place, PlaceDto> _mapperPlaceTo;

    public MobileTaskController(WarehouseContext context, IOutTaskData outTaskData, IMapper<OutTask, OutTaskDto> mapperTo, IMapper<OutTaskDto, OutTask> mapperFrom, 
        IMapper<ProductType, ProductTypeDto> mapperProductTypeTo, IMapper<Place, PlaceDto> mapperPlaceTo)
    {
        _context = context;
        _outTaskData = outTaskData;
        _mapperTo = mapperTo;
        _mapperFrom = mapperFrom;
        _mapperProductTypeTo = mapperProductTypeTo;
        _mapperPlaceTo = mapperPlaceTo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = (await _outTaskData.GetAllAsync(true)).OrderBy(ot => ot.CreatedDateTime);
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
        var productType = await _context.ProductTypes.FirstOrDefaultAsync(pt => pt.FormatNumber == format);
        if (productType is null)
            return NotFound();
        return Ok(_mapperProductTypeTo.Map(productType));
    }

    [HttpGet("producttypeplaces/{productTypeId:int}")]
    public async Task<IActionResult> GetRecommendedLoadPlaces(int productTypeId)
    {
        var query = _context.Places.Include(p => p.ProductType).Where(p => p.ProductTypeId == null || (p.ProductTypeId == productTypeId && p.Count < p.Size) );
        var places = await query.OrderByDescending(p => p.Count).ToArrayAsync();
        return Ok(places.Select(p => _mapperPlaceTo.Map(p)));
    }

    [HttpGet("place/{number:int}")]
    public async Task<IActionResult> FindPlaceByNumber(int number)
    {
        var place = await _context.Places.FirstOrDefaultAsync(p => p.Number == number);
        if (place is null)
            return NotFound();
        return Ok(_mapperPlaceTo.Map(place));
    }

    [HttpGet("load/{productTypeId:int}/{placeId:int}/{count:int}")]
    public async Task<IActionResult> LoadProductToPlace(int productTypeId, int placeId, int count)
    {
        var place = await _context.Places.FirstOrDefaultAsync(p => p.Id == placeId);
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
}

