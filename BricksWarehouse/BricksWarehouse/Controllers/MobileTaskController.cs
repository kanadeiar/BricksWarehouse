using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BricksWarehouse.Controllers;

[Route("api/[controller]"), ApiController]
public class MobileTaskController : ControllerBase
{
    private readonly IOutTaskData _outTaskData;
    private readonly IMapper<OutTask, OutTaskDto> _mapperTo;
    private readonly IMapper<OutTaskDto, OutTask> _mapperFrom;
    public MobileTaskController(IOutTaskData outTaskData, IMapper<OutTask, OutTaskDto> mapperTo, IMapper<OutTaskDto, OutTask> mapperFrom)
    {
        _outTaskData = outTaskData;
        _mapperTo = mapperTo;
        _mapperFrom = mapperFrom;
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



}

