﻿using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.Services;
using BricksWarehouse.Mobile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BricksWarehouse.Mobile.ViewModels.Trash
{
    public class TrashProductTypeViewModel : ViewModel
    {
        #region Данные

        private readonly ProductTypeClient _productTypeClient;

        #endregion

        #region Свойства

        public ObservableCollection<ProductType> ProductTypes { get; set; } = new ObservableCollection<ProductType>();

        private ProductType _SelectedProductType;
        /// <summary> Выбранный вид товаров </summary>
        public ProductType SelectedProductType
        {
            get => _SelectedProductType;
            set => Set(ref _SelectedProductType, value);
        }

        private bool _refreshingProductTypes;
        /// <summary> Обновление данных </summary>
        public bool RefreshingProductTypes
        {
            get => _refreshingProductTypes;
            set => Set(ref _refreshingProductTypes, value);
        }

        private string _Title = "Редактирование видов";
        /// <summary> Название приложения </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        public TrashProductTypeViewModel(ProductTypeClient productTypeClient)
        {
            _productTypeClient = productTypeClient;

            Task.Run(async () =>
            {
                await UpdateDataAsync();
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
            await UpdateDataAsync();
            RefreshingProductTypes = false;
        }

        #endregion

        #region Вспомогательные

        public async Task UpdateDataAsync()
        {
            var productTypes = await _productTypeClient.GetTrashed();
            ProductTypes.Clear();
            foreach (var productType in productTypes)
                ProductTypes.Add(productType);
        }

        #endregion
    }
}
