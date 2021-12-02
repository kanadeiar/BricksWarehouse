using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BricksWarehouse.Mobile.ViewModels.Edit
{
    public class EditProductTypeViewModel : ViewModel
    {
        #region Данные

        public Action<ProductType> ContinueAction;

        public INavigation Navigation { get; set; }

        #endregion

        #region Свойства

        private string _name;
        /// <summary> Название </summary>
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        private int _FormatNumber;
        /// <summary> Номер формата </summary>
        public int FormatNumber
        {
            get => _FormatNumber;
            set => Set(ref _FormatNumber, value);
        }
        private int _Order;
        /// <summary> Сортировка </summary>
        public int Order
        {
            get => _Order;
            set => Set(ref _Order, value);
        }
        private int _Units;
        /// <summary> Количество едениц в пачке </summary>
        public int Units
        {
            get => _Units;
            set => Set(ref _Units, value);
        }
        private double _Volume;
        /// <summary> Объем, занимаемый одной пачкой </summary>
        public double Volume
        {
            get => _Volume;
            set => Set(ref _Volume, value);
        }
        private double _Weight;
        /// <summary> Вес одной пачки </summary>
        public double Weight
        {
            get => _Weight;
            set => Set(ref _Weight, value);
        }
        private bool _IsDelete;
        /// <summary> Метка удаления вида товаров </summary>
        public bool IsDelete
        {
            get => _IsDelete;
            set => Set(ref _IsDelete, value);
        }

        private string _Title = "Редактирование вида";
        /// <summary> Название вьюхи </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        public EditProductTypeViewModel()
        {

        }

        #region Команды

        private ICommand _OKProductTypeCommand;
        /// <summary> Успешное завершение редактирования </summary>
        public ICommand OKProductTypeCommand => _OKProductTypeCommand ??= new Command(OnOKProductTypeCommandExecuted);
        private async void OnOKProductTypeCommandExecuted(object p)
        {
            if (string.IsNullOrEmpty(Name))
            {
                await App.Current.MainPage.DisplayAlert("Ошибка ввода", "Необходимо ввести название вида товаров", "OK");
                return;
            }

            await Navigation.PopAsync();
            var completed = new ProductType
            {
                Name = Name,
                FormatNumber = FormatNumber,
                Order = Order,
                Units = Units,
                Volume = Volume,
                Weight = Weight,
                IsDelete = IsDelete,
            };
            ContinueAction.Invoke(completed);
        }

        #endregion

        #region Вспомогательное

        /// <summary> Установка данных на вьюмодель </summary>
        public void SetData(ProductType productType)
        {
            Name = productType.Name;
            FormatNumber = productType.FormatNumber;
            Order = productType.Order;
            Units = productType.Units;
            Volume = productType.Volume;
            Weight = productType.Weight;
            IsDelete = productType.IsDelete;
        }

        #endregion
    }
}
