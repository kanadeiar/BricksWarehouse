using Microsoft.AspNetCore.Mvc;

namespace BricksWarehouse.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class MobilePlaceController : ControllerBase
    {
        private readonly IPlaceData _placeData;
        private readonly IMapper<Place, PlaceDto> _mapperTo;
        private readonly IMapper<PlaceDto, Place> _mapperFrom;
        public MobilePlaceController(IPlaceData placeData, IMapper<Place, PlaceDto> mapperTo, IMapper<PlaceDto, Place> mapperFrom)
        {
            _placeData = placeData;
            _mapperTo = mapperTo;
            _mapperFrom = mapperFrom;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var places = (await _placeData.GetAllAsync(true)).OrderBy(pt => pt.Order);
            return Ok(places.Select(p => _mapperTo.Map(p)));
        }

        [HttpGet("trashed")]
        public async Task<IActionResult> GetTrashed()
        {
            var places = (await _placeData.GetAllAsync(true, true)).OrderBy(p => p.Order);
            return Ok(places.Select(p => _mapperTo.Map(p)));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var place = await _placeData.GetAsync(id);
            if (place is null)
                return NotFound();
            return Ok(_mapperTo.Map(place));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PlaceDto dto)
        {
            var place = _mapperFrom.Map(dto);
            place.Id = default;
            var id = await _placeData.AddAsync(place);
            var result = await _placeData.GetAsync(id);
            if (result is null)
                return NotFound();
            return Ok(_mapperTo.Map(result));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PlaceDto dto)
        {
            var place = _mapperFrom.Map(dto);
            await _placeData.UpdateAsync(place);
            var result = await _placeData.GetAsync(place.Id);
            if (result is null)
                return NotFound();
            return Ok(_mapperTo.Map(result));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _placeData.GetAsync(id);
            var result = await _placeData.DeleteAsync(id);
            if (result == false)
                return NotFound(false);
            return Ok(_mapperTo.Map(deleted));
        }

        [HttpGet("totrash/{id:int}")]
        public async Task<IActionResult> ToTrash(int id)
        {
            var result = await _placeData.TrashAsync(id);
            return result ? Ok(true) : NotFound(false);
        }

        [HttpGet("fromtrash/{id:int}")]
        public async Task<IActionResult> FromTrash(int id)
        {
            var result = await _placeData.TrashAsync(id, true);
            return result ? Ok(true) : NotFound(false);
        }
    }
}
