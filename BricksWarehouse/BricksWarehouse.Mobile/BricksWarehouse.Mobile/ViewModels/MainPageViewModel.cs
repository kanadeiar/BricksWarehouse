using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.Services;
using BricksWarehouse.Mobile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BricksWarehouse.Mobile.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        #region Данные

        private readonly ProductTypeClient _productTypeClient;

        #endregion

        #region Свойства

        public ObservableCollection<ProductTypeView> ProductTypes { get; set; } = new ObservableCollection<ProductTypeView>();

        private bool _refreshingProductTypes;
        /// <summary> Обновление данных </summary>
        public bool RefreshingProductTypes
        {
            get => _refreshingProductTypes;
            set => Set(ref _refreshingProductTypes, value);
        }

        private string _Title = "Обзор склада";


        /// <summary> Название приложения </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        public MainPageViewModel(ProductTypeClient productTypeClient)
        {
            _productTypeClient = productTypeClient;

            Task.Run(async () =>
            {
                await UpdateProductTypes();
                RefreshingProductTypes = false;
            });
        }

        #region Команды



        private ICommand _UpdateProductTypesCommand;
        /// <summary> Обновить </summary>
        public ICommand UpdateProductTypesCommand => _UpdateProductTypesCommand ??= new Command(OnUpdateProductTypesCommandExecuted);
        private async void OnUpdateProductTypesCommandExecuted(object p)
        {
            RefreshingProductTypes = true;
            await UpdateProductTypes();
            RefreshingProductTypes = false;
        }

        #endregion

        #region Вспомогательное

        public async Task UpdateProductTypes()
        {
            var productTypes = await _productTypeClient.GetAll();
            ProductTypes.Clear();
            foreach (var productType in productTypes)
                ProductTypes.Add( new ProductTypeView
                {
                    FormatNumber = productType.FormatNumber,
                    Name = productType.Name,
                    PlacesCount = productType.Places.Count,
                    CountUnits = productType.Places.Count * productType.Units,
                });
        }

        #endregion
    }

    #region Вспомогательные вьюмодели

    public class ProductTypeView
    {
        public int FormatNumber { get; set; }
        public string Name { get; set; }
        public int PlacesCount { get; set; }
        public int CountUnits { get; set; }
    }

    #endregion
}
