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
    private readonly IMapper<ProductType, ProductTypeDto> _mapperProductTo;

    public MobileTaskController(WarehouseContext context, IOutTaskData outTaskData, IMapper<OutTask, OutTaskDto> mapperTo, IMapper<OutTaskDto, OutTask> mapperFrom, IMapper<ProductType, ProductTypeDto> mapperProductTo)
    {
        _context = context;
        _outTaskData = outTaskData;
        _mapperTo = mapperTo;
        _mapperFrom = mapperFrom;
        _mapperProductTo = mapperProductTo;
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

    [HttpGet("code/{format:int}")]
    public async Task<IActionResult> FindProductTypeByCode(int format)
    {
        var productType = await _context.ProductTypes.FirstOrDefaultAsync(pt => pt.FormatNumber == format);
        if (productType is null)
            return NotFound();
        return Ok(_mapperProductTo.Map(productType));
    }

}

