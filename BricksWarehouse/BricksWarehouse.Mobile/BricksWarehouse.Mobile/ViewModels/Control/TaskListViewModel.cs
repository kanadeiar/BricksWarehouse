using System;
using System.Collections.Generic;
using System.Text;
using BricksWarehouse.Mobile.ViewModels.Base;

namespace BricksWarehouse.Mobile.ViewModels.Control
{
    public class TaskListViewModel : ViewModel
    {
        #region Данные



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

        public TaskListViewModel()
        {
            
        }

        #region Команды



        #endregion

        #region Вспомогательное



        #endregion
    }
}
