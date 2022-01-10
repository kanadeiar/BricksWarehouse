using System.Linq;

namespace BricksWarehouse.Services
{
    public class FillingsInfoService
    {
        private readonly IPlaceData _placeData;
        public FillingsInfoService(IPlaceData placeData)
        {
            _placeData = placeData;
        }

        public async Task<IEnumerable<FillingInfoWebModel>> GetFillings()
        {
            var places = await _placeData.GetAllAsync(true);
            var results = places.Select(p => new FillingInfoWebModel
            {
                Id = p.Id,
                Name = p.Name,
                Number = p.Number,
                Occupied = p.Count,
                Status = Place.GetNamePlaceStatus(p.PlaceStatus),
                Size = p.Size,
            }).ToList();
            foreach (var place in results)
            {
                place.Free = place.Size - place.Occupied;
                if (place.Free < 0)
                    place.Free = 0;
                place.Filling = Convert.ToInt32(place.Size != 0 ? place.Occupied / (double)place.Size * 100.0 : 0.0);
                place.Freeing = Convert.ToInt32(place.Size != 0 ? place.Free / (double)place.Size * 100.0 : 0.0);
                if (place.Occupied == 0)
                    place.Sost = "Пусто";
                else if (place.Occupied == place.Size)
                    place.Sost = "Заполнен";
                else if (place.Occupied >= place.Size)
                    place.Sost = "Переполнен";
                else if (place.Occupied == 1)
                    place.Sost = "Мало наполнен";
                else
                    place.Sost = "Норма";
            }
            return results.OrderBy(p => p.Name);
        }
    }

    public class FillingInfoWebModel
    {
        public int Id { get; set; }
        [Display(Name = "Место хранения")]
        public string Name { get; set; }
        [Display(Name = "Номер места")]
        public int Number { get; set; }
        [Display(Name = "Заполнение %")]
        public int Filling { get; set; }
        [Display(Name = "Свободно мест")]
        public int Free { get; set; }
        [Display(Name = "Свободных мест %")]
        public int Freeing { get; set; }
        [Display(Name = "Занято мест")]
        public int Occupied { get; set; }
        [Display(Name = "Всего мест")]
        public int Size { get; set; }
        [Display(Name = "Статус места")]
        public string Status { get; set; }
        [Display(Name = "Состояние места")]
        public string Sost { get; set; }
    }
}
