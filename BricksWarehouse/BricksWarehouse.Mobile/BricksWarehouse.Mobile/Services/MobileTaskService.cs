using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BricksWarehouse.Domain.Models;

namespace BricksWarehouse.Mobile.Services
{
    /// <summary> Сервис заданий в мобильном устройстве </summary>
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

        /// <summary> Получить все задания для оператора склада </summary>
        /// <param name="onlynoncompleted">Только незавершенные задания</param>
        /// <returns>Список заданий</returns>
        public async Task<IEnumerable<OutTask>> GetAllOutTasks(bool onlynoncompleted = false)
        {
            var tasks = (onlynoncompleted) 
                ? (await _taskClient.GetAll()).Where(t => !t.IsCompleted)
                : await _taskClient.GetAll();
            return tasks;
        }

        /// <summary> Получить одно задание для оператора </summary>
        /// <param name="id">Идентификатор задания</param>
        /// <returns>Задание</returns>
        public async Task<OutTask> GetOneOutTask(int id)
        {
            var task = await _taskClient.GetById(id);
            return task;
        }

        /// <summary> Установить задание по его номеру </summary>
        /// <param name="numberTask">Номер задания</param>
        /// <returns>Это задание</returns>
        public async Task SetTaskWithNumber(int numberTask)
        {
            if (numberTask == 0)
            {
                OutTask = new OutTask{ Number = 0 };
                return;
            }
            var task = (await _taskClient.GetAll()).FirstOrDefault(t => t.Number == numberTask);
            if (task != null)
            {
                OutTask = task;
            }
        }

        /// <summary> Получить вид товара по номеру </summary>
        /// <param name="formatNumber">Номер товара</param>
        /// <returns>Вид товара</returns>
        public async Task<ProductType> GetProductTypeByNumber(int formatNumber)
        {
            var productType = await _taskClient.GetProductTypeByFormat(formatNumber);
            return productType;
        }

        /// <summary> Начало задания приема товара на склад </summary>
        /// <param name="productType">Вид товара</param>
        public void StartLoadTask(ProductType productType)
        {
            ProductType = productType;
        }

        /// <summary> Получить список рекомендукмых мест при загрузке товара на склад </summary>
        /// <param name="produtctTypeId">Идентификатор вида товара</param>
        /// <returns>Рекомендукмый список, вверху - наиболее подходящее</returns>
        public async Task<IEnumerable<Place>> GetRecommendedLoadPlaces(int produtctTypeId)
        {
            var places = await _taskClient.GetRecommendedLoadPlaces(produtctTypeId);
            return places;
        }

        /// <summary> Получить место хранения товаров по номеру </summary>
        /// <param name="placeNumber">Номер места</param>
        /// <returns>Место</returns>
        public async Task<Place> GetPlaceByNumber(int placeNumber)
        {
            var place = await _taskClient.GetPlaceByNumber(placeNumber);
            return place;
        }

        /// <summary> Продолжение выполнения задания по приему товара на склад </summary>
        /// <param name="place">Место хранения товара</param>
        public void BeginLoadTask(Place place)
        {
            Place = place;
        }

        /// <summary> Завершение выполнения задания по приему товара на склад </summary>
        /// <param name="count">Количество товара</param>
        /// <returns>Обновленное место хранения товара</returns>
        public async Task<Place> EndLoadTask(int count)
        {
            var place = await _taskClient.LoadProductToPlace(ProductType.Id, Place.Id, count);
            return place;
        }

        /// <summary> Получить список рекомендуемых мест для отгрузки товара </summary>
        /// <param name="productTypeId">Вид товара</param>
        /// <returns>Список рекомендуемых, вверху - наиболее подходящее</returns>
        public async Task<IEnumerable<Place>> GetRecommendedShipmentPlaces(int productTypeId)
        {
            var places = await _taskClient.GetRecommendedShipmentPlaces(productTypeId);
            return places;
        }

        /// <summary> Начало выполнения задания по отгрузке товара со склада </summary>
        /// <param name="task">Задание</param>
        public void StartShippingTask(OutTask task)
        {
            OutTask = task;
            ProductType = task.ProductType;
        }

        /// <summary> Продолжение выполнения задания по отгрузке товара со склада </summary>
        /// <param name="place">Место хранения, с которого сгружать товар</param>
        public void BeginShippingTask(Place place)
        {
            Place = place;
        }

        /// <summary> Завершение выполнения задания по отгрузке товара со склада </summary>
        /// <param name="count">Количество товара</param>
        /// <returns>Обновленное место хранения товара</returns>
        public async Task<Place> EndShippingTask(int count)
        {
            var place = await _taskClient.ShipmentProductToPlace(Place.Id, OutTask.Id, count);
            return place;
        }
    }
}
