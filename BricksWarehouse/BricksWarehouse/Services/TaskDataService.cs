namespace BricksWarehouse.Services
{
    public class TaskDataService
    {
        private readonly IPlaceData _placeData;
        private readonly IOutTaskData _outTaskData;
        public TaskDataService(IPlaceData placeData, IOutTaskData outTaskData)
        {
            _placeData = placeData;
            _outTaskData = outTaskData;
        }

        /// <summary> Получить список рекомендованных мест хранений товаров для задания по загрузке склада принимаемым товаром </summary>
        /// <param name="productTypeId">Вид товара</param>
        /// <returns>Список рекомендаций, в самом верху - самый рекомендуемый</returns>
        public async Task<IEnumerable<Place>> GetRecommendedLoadPlaces(int productTypeId) 
        {
            var places = (await _placeData.GetAllAsync(true))
                .Where(p => p.ProductTypeId == null || (p.ProductTypeId == productTypeId && p.Count < p.Size))
                .OrderByDescending(p => p.Count);
            return places;
        }

        /// <summary> Получить список рекомендованных мест хранений товаров для задания по отгрузке товара со склада в грузовик </summary>
        /// <param name="productTypeId">Вид товара</param>
        /// <returns>Список рекомендаций, в самом верху - самый рекомендуемый</returns>
        public async Task<IEnumerable<Place>> GetRecommendedShipmentPlaces(int productTypeId)
        {
            var places = (await _placeData.GetAllAsync(true))
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
                if (place.Count > place.Size)
                    place.Count = place.Size;
                place.LastDateTime = DateTime.Now;
                await _placeData.UpdateAsync(place);
                return await _placeData.GetAsync(placeId);
            }
            return null;
        }

        /// <summary> Отгрузка товара со склада с места хранения товаров при выполнении задания по отгрузке товаров со склада </summary>
        /// <param name="placeId">Место хранения товара</param>
        /// <param name="taskId">Задание на отгрузку</param>
        /// <param name="count">Количество</param>
        /// <returns>Обновленное место в случае успеха</returns>
        public async Task<Place?> ShipmentProductFromPlace(int placeId, int taskId, int count)
        {
            var task = await _outTaskData.GetAsync(taskId);
            var place = await _placeData.GetAsync(placeId);
            if (task is null || place is null || task.ProductTypeId is null || place.ProductTypeId is null)
                return null;
            if (count > task.Count - task.Loaded)
                count = task.Count - task.Loaded;
            if (count > place.Count)
                count = place.Count;
            if (task.ProductTypeId == place.ProductTypeId)
            {
                task.Loaded += count;
                if (task.Loaded >= task.Count)
                    task.IsCompleted = true;
                await _outTaskData.UpdateAsync(task);
                place.Count -= count;
                if (place.Count <= 0)
                {
                    place.ProductTypeId = null;
                }
                await _placeData.UpdateAsync(place);
                return await _placeData.GetAsync(placeId);
            }
            return null;
        }
    }
}
