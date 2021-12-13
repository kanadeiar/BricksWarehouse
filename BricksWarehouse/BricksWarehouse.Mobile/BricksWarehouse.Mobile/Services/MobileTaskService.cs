using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BricksWarehouse.Domain.Models;

namespace BricksWarehouse.Mobile.Services
{
    /// <summary> Сервис выполнения заданий в мобильном устройстве </summary>
    public class MobileTaskService
    {
        private readonly TaskClient _taskClient;

        /// <summary> Выполняемое задание </summary>
        public OutTask OutTask { get; set; }
        /// <summary> Вид товаров </summary>
        public ProductType ProductType { get; set; }
        /// <summary> Место хранений товаров </summary>
        public Place Place { get; set; }

        public MobileTaskService(TaskClient taskClient)
        {
            _taskClient = taskClient;
        }

        public async Task<IEnumerable<OutTask>> GetNonCompletedAllOutTasks()
        {
            var tasks = (await _taskClient.GetAll()).Where(tc => tc.IsCompleted);
            return tasks;
        }

        public async Task<IEnumerable<OutTask>> GetAllOutTasks()
        {
            var tasks = await _taskClient.GetAll();
            return tasks;
        }

        public async Task<OutTask> GetOneOutTask(int id)
        {
            var task = await _taskClient.GetById(id);
            return task;
        }

        public async Task<OutTask> GetTaskByNumber(int numberTask)
        {
            var task = (await _taskClient.GetAll()).FirstOrDefault(t => t.Number == numberTask);
            return task;
        }

        public async Task<ProductType> GetProductTypeByNumber(int formatNumber)
        {
            var productType = await _taskClient.GetProductTypeByFormat(formatNumber);
            return productType;
        }

        public async Task<IEnumerable<Place>> GetRecommendedLoadPlaces(int produtctTypeId)
        {
            var places = await _taskClient.GetRecommendedLoadPlaces(produtctTypeId);
            return places;
        }

        public async Task<Place> GetPlaceByNumber(int placeNumber)
        {
            var place = await _taskClient.GetPlaceByNumber(placeNumber);
            return place;
        }

        public async Task<Place> LoadProductToPlace(int productTypeId, int placeId, int count)
        {
            var place = await _taskClient.LoadProductToPlace(productTypeId, placeId, count);
            return place;
        }

        public async Task<IEnumerable<Place>> GetRecommendedShipmentPlaces(int productTypeId)
        {
            var places = await _taskClient.GetRecommendedShipmentPlaces(productTypeId);
            return places;
        }

        public async Task<Place> ShipmenetProductToPlace(int placeId, int taskId, int count)
        {
            var place = await _taskClient.ShipmentProductToPlace(placeId, taskId, count);
            return place;
        }
    }
}
