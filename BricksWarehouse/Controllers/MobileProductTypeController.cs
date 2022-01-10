using Microsoft.AspNetCore.Mvc;

namespace BricksWarehouse.Controllers;

[Route("api/[controller]"), ApiController]
public class MobileProductTypeController : ControllerBase
{
    private readonly IProductTypeData _productTypeData;
    private readonly IMapper<ProductType, ProductTypeDto> _mapperTo;
    private readonly IMapper<ProductTypeDto, ProductType> _mapperFrom;
    public MobileProductTypeController(IProductTypeData productTypeData, IMapper<ProductType, ProductTypeDto> mapperTo, IMapper<ProductTypeDto, ProductType> mapperFrom)
    {
        _productTypeData = productTypeData;
        _mapperTo = mapperTo;
        _mapperFrom = mapperFrom;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var productTypes = (await _productTypeData.GetAllAsync(true)).OrderBy(pt => pt.Order);
        return Ok(productTypes.Select(p => _mapperTo.Map(p)));
    }

    [HttpGet("trashed")]
    public async Task<IActionResult> GetTrashed()
    {
        var productTypes = (await _productTypeData.GetAllAsync(true, true)).OrderBy(p => p.Order);
        return Ok(productTypes.Select(p => _mapperTo.Map(p)));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var productType = await _productTypeData.GetAsync(id);
        if (productType is null)
            return NotFound();
        return Ok(_mapperTo.Map(productType));
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] ProductTypeDto dto)
    {
        var productType = _mapperFrom.Map(dto);
        productType.Id = default;
        var id = await _productTypeData.AddAsync(productType);
        var result = await _productTypeData.GetAsync(id);
        if (result is null)
            return NotFound();
        return Ok(_mapperTo.Map(result));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductTypeDto dto)
    {
        var productType = _mapperFrom.Map(dto);
        await _productTypeData.UpdateAsync(productType);
        var result = await _productTypeData.GetAsync(productType.Id);
        if (result is null)
            return NotFound();
        return Ok(_mapperTo.Map(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productTypeData.GetAsync(id);
        var result = await _productTypeData.DeleteAsync(id);
        if (result == false)
            return NotFound(false);
        return Ok(_mapperTo.Map(deleted));
    }

    [HttpGet("totrash/{id:int}")]
    public async Task<IActionResult> ToTrash(int id)
    {
        var result = await _productTypeData.TrashAsync(id);
        return result ? Ok(true) : NotFound(false);
    }

    [HttpGet("fromtrash/{id:int}")]
    public async Task<IActionResult> FromTrash(int id)
    {
        var result = await _productTypeData.TrashAsync(id, true);
        return result ? Ok(true) : NotFound(false);
    }
}

