namespace BricksWarehouse.Services
{
    public class GetTaskDataService
    {
        private readonly IPlaceData _placeData;

        public GetTaskDataService(IPlaceData placeData)
        {
            _placeData = placeData;

        }

        /// <summary> Получить список рекомендованных мест хранений товаров для задания по загрузке склада принимаемым товаром </summary>
        /// <param name="productTypeId">Вид товара</param>
        /// <returns>Список рекомендаций, в самом верху - самый рекомендуемый</returns>
        public async Task<IEnumerable<Place>> GetRecommendedLoadPlaces(int productTypeId) 
        {
            var places = (await _placeData.GetAllAsync())
                .Where(p => p.ProductTypeId == null || (p.ProductTypeId == productTypeId && p.Count < p.Size))
                .OrderByDescending(p => p.Count);
            return places;
        }

        /// <summary> Получить список рекомендованных мест хранений товаров для задания по отгрузке товара со склада в грузовик </summary>
        /// <param name="productTypeId">Вид товара</param>
        /// <returns>Список рекомендаций, в самом верху - самый рекомендуемый</returns>
        public async Task<IEnumerable<Place>> GetRecommendedShipmentPlaces(int productTypeId)
        {
            var places = (await _placeData.GetAllAsync())
                .Where(p => p.ProductTypeId == productTypeId && p.Count > 0)
                .OrderBy(p => p.LastDateTime);
            return places;
        }

        /// <summary> Загрузка товара на место хранения товаров при выполнении задания по загрузке склада принимаемым товаром </summary>
        /// <param name="productTypeId">Вид товара</param>
        /// <param name="placeId">Место хранения</param>
        /// <param name="count">Количество</param>
        /// <returns>Обновленное место в случае успеха</returns>
        public async Task<Place?> LoadProductToPlace(int productTypeId, int placeId, int count)
        {
            var place = await _placeData.GetAsync(placeId);
            if (place is null)
                return null;
            if (place.ProductTypeId is null)
                place.ProductTypeId = productTypeId;
            if (place.ProductTypeId == productTypeId)
            {
                place.Count += count;
                place.LastDateTime = DateTime.Now;
                return place;
            }
            return null;
        }
    }
}
