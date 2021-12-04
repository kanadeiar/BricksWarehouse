using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.Services;
using BricksWarehouse.Mobile.ViewModels.Base;
using BricksWarehouse.Mobile.Views.Edit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BricksWarehouse.Mobile.ViewModels.Edit
{
    public class ProductTypeViewModel : ViewModel
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

        public ProductTypeViewModel(ProductTypeClient productTypeClient)
        {
            _productTypeClient = productTypeClient;

            Task.Run(async () =>
            {
                await UpdateDataAsync();
                RefreshingProductTypes = false;
            });
        }

        #region Команды

        private ICommand _AddProductTypeCommand;
        /// <summary> Создать вид товаров </summary>
        public ICommand AddProductTypeCommand => _AddProductTypeCommand ??= new Command(OnAddProductTypeCommandExecuted);
        private async void OnAddProductTypeCommandExecuted(object p)
        {
            var newest = new ProductType
            {
                Id = 0,
                Name = string.Empty,
            };
            await Application.Current.MainPage.Navigation.PushAsync(new EditProductTypePage("Создание вида", newest, AddProductTypeCompleted));
        }
        private async void AddProductTypeCompleted(ProductType adding)
        {
            adding.Id = 0;
            await _productTypeClient.Add(adding);
            ProductTypes.Add(adding);
        }

        private ICommand _EditProductTypeCommand;
        /// <summary> Редактировать вид товаров </summary>
        public ICommand EditProductTypeCommand => _EditProductTypeCommand ??= new Command(OnEditProductTypeCommandExecuted, CanEditProductTypeCommandExecuted);
        private bool CanEditProductTypeCommandExecuted(object p) => p is ProductType;
        private async void OnEditProductTypeCommandExecuted(object p)
        {
            var selected = p as ProductType;
            await Application.Current.MainPage.Navigation.PushAsync(new EditProductTypePage("Редактирование вида", selected, EditProductTypeCompleted));
        }
        private async void EditProductTypeCompleted(ProductType editing)
        {
            var selected = SelectedProductType;
            selected.Name = editing.Name;
            selected.FormatNumber = editing.FormatNumber;
            selected.Order = editing.Order;
            selected.Units = editing.Units;
            selected.Volume = editing.Volume;
            selected.Weight = editing.Weight;
            selected.IsDelete = editing.IsDelete;
            await _productTypeClient.Update(selected);
        }

        private ICommand _DeleteProductTypeCommand;
        /// <summary> Удалить вид товаров </summary>
        public ICommand DeleteProductTypeCommand => _DeleteProductTypeCommand ??= new Command(OnDeleteProductTypeCommandExecuted, CanDeleteProductTypeCommandExecuted);
        private bool CanDeleteProductTypeCommandExecuted(object p) => p is ProductType;
        private async void OnDeleteProductTypeCommandExecuted(object p)
        {
            if (p is ProductType productType)
            {
                await _productTypeClient.ToTrash(productType.Id);
                ProductTypes.Remove(productType);
                SelectedProductType = null;
            }
        }

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
            var productTypes = await _productTypeClient.GetAll();
            ProductTypes.Clear();
            foreach (var productType in productTypes)
                ProductTypes.Add(productType);
        }

        #endregion
    }
}
