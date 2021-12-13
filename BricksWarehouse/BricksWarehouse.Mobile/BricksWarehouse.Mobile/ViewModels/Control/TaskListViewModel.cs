using System;
using System.Collections.Generic;
using System.Text;
using BricksWarehouse.Mobile.Services;
using BricksWarehouse.Mobile.ViewModels.Base;

namespace BricksWarehouse.Mobile.ViewModels.Control
{
    public class TaskListViewModel : ViewModel
    {
        #region Данные

        private readonly MobileTaskService _mobileTaskService;

        #endregion

        #region Свойства


        private string _title = "Управление складом";
        /// <summary> Заголовок </summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        public TaskListViewModel(MobileTaskService mobileTaskService)
        {
            _mobileTaskService = mobileTaskService;
        }

        #region Команды



        #endregion

        #region Вспомогательное



        #endregion
    }

    #region Вспомогательные вьюмодели

    public class OutTaskView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string ProductTypeName { get; set; }
        public string TruckNumber { get; set; }
        public string CountString { get; set; }
    }

    #endregion
}
