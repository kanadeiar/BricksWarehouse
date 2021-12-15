using System;
using System.Collections.Generic;
using System.Text;
using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.Services;
using BricksWarehouse.Mobile.ViewModels.Base;
using Xamarin.Forms;

namespace BricksWarehouse.Mobile.ViewModels.Control
{
    public class TaskDetailViewModel : ViewModel
    {
        #region Данные

        private readonly MobileTaskService _MobileTaskService;
        private readonly ParseQrService _ParseQrService;
        public INavigation Navigation { get; set; }

        #endregion

        #region Свойства

        private string _Name;
        /// <summary> Название задания </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private int _Number;
        /// <summary> Номер задания </summary>
        public int Number
        {
            get => _Number;
            set => Set(ref _Number, value);
        }

        private string _ProductTypeName;
        /// <summary> Название вида товаров задания </summary>
        public string ProductTypeName
        {
            get => _ProductTypeName;
            set => Set(ref _ProductTypeName, value);
        }

        private string _TruckNumber;
        /// <summary> Номер грузовика </summary>
        public string TruckNumber
        {
            get => _TruckNumber;
            set => Set(ref _TruckNumber, value);
        }

        private string _LoadedString;
        /// <summary> Количество загруженых уже товаров </summary>
        public string LoadedString
        {
            get => _LoadedString;
            set => Set(ref _LoadedString, value);
        }

        private string _CreatedDateTime;
        /// <summary> Дата и время создания задания </summary>
        public string CreatedDateTime
        {
            get => _CreatedDateTime;
            set => Set(ref _CreatedDateTime, value);
        }

        private string _Comment;
        /// <summary> Комментарий </summary>
        public string Comment
        {
            get => _Comment;
            set => Set(ref _Comment, value);
        }

        private string _IsCompletedString;
        /// <summary> Статус выполнения задания </summary>
        public string IsCompletedString
        {
            get => _IsCompletedString;
            set => Set(ref _IsCompletedString, value);
        }

        private string _Title = "Детали задания";
        /// <summary> Название вьюхи </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        public TaskDetailViewModel(MobileTaskService MobileTaskService, ParseQrService parseQrService)
        {
            _MobileTaskService = MobileTaskService;
            _ParseQrService = parseQrService;
        }

        #region Команды



        #endregion

        #region Вспомогательные

        /// <summary> Установка данных на вьюмодель </summary>
        public void SetData(OutTask task)
        {
            if (task.Id != 0)
            {
                Name = task.Name;
                Number = task.Number;
                ProductTypeName = task.ProductType?.Name;
                LoadedString = $"{task.Loaded} / {task.Count}";
                TruckNumber = task.TruckNumber;
                CreatedDateTime = task.CreatedDateTime.ToString("F");
                Comment = task.Comment;
                IsCompletedString = task.IsCompleted ? "Выполнено" : "В процессе выполнения";
            }
            else
            {
                Name = "Заполнение склада товарами";
                Number = 0;
                ProductTypeName = "Тот, который принимается на склад";
                LoadedString = "Выполняется постоянно";
                CreatedDateTime = "Выполняется постоянно";
                IsCompletedString = "Постоянно в процессе выполнения";
            }
        }

        #endregion
    }
}
