using BricksWarehouse.Mobile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BricksWarehouse.Mobile.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        #region Данные

        #endregion

        #region Свойства



        private string _Title = "Склад блоков";
        /// <summary> Название приложения </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        public MainPageViewModel()
        {

        }

        #region Команды



        #endregion

        #region Вспомогательное



        #endregion
    }
}
