using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BricksWarehouse.Mobile.ViewModels.Edit
{
    public class EditPlaceViewModel : ViewModel
    {
        #region Данные

        public Action<Place> ContinueAction;

        public INavigation Navigation { get; set; }

        #endregion

        #region Свойства

        private string _Name;
        /// <summary> Название </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private int _Order;
        /// <summary> Сортировка </summary>
        public int Order
        {
            get => _Order;
            set => Set(ref _Order, value);
        }
        private int _Number;
        /// <summary> Номер </summary>
        public int Number
        {
            get => _Number;
            set => Set(ref _Number, value);
        }

        public ObservableCollection<ProductType> ProductTypes { get; set; } = new ObservableCollection<ProductType>();
        private ProductType _SelectedProductType;
        /// <summary> Выбранный вид товаров </summary>
        public ProductType SelectedProductType
        {
            get => _SelectedProductType;
            set => Set(ref _SelectedProductType, value);
        }

        private int _Count;
        /// <summary> Количество </summary>
        public int Count
        {
            get => _Count;
            set => Set(ref _Count, value);
        }
        private int _Size;
        /// <summary> Вместимость </summary>
        public int Size
        {
            get => _Size;
            set => Set(ref _Size, value);
        }
        private DateTime _LastDateTime;
        /// <summary> Дата и время помещения последнего товара </summary>
        public DateTime LastDateTime
        {
            get => _LastDateTime;
            set => Set(ref _LastDateTime, value);
        }

        public IEnumerable<PlaceStatus> PlaceStatuses { get; set; } = Enumerable.Empty<PlaceStatus>();
        private PlaceStatus _PlaceStatus;
        /// <summary> Статус места </summary>
        public PlaceStatus PlaceStatus
        {
            get => _PlaceStatus;
            set => Set(ref _PlaceStatus, value);
        }

        private string _Comment;
        /// <summary> Комментарий </summary>
        public string Comment
        {
            get => _Comment;
            set => Set(ref _Comment, value);
        }
        private bool _IsDelete;
        /// <summary> Метка удаления вида товаров </summary>
        public bool IsDelete
        {
            get => _IsDelete;
            set => Set(ref _IsDelete, value);
        }

        private string _Title = "Редактирование места";
        /// <summary> Название вьюхи </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        public EditPlaceViewModel()
        {
            PlaceStatuses = new List<PlaceStatus>() { PlaceStatus.Default, PlaceStatus.Collect, PlaceStatus.Wait, PlaceStatus.Delivery };
        }

        #region Команды

        private ICommand _OKPlaceCommand;
        /// <summary> Успешное завершение редактирования </summary>
        public ICommand OKPlaceCommand => _OKPlaceCommand ??= new Command(OnOKPlaceCommandExecuted);
        private async void OnOKPlaceCommandExecuted(object p)
        {
            if (string.IsNullOrEmpty(Name) || Number <= 0)
            {
                await App.Current.MainPage.DisplayAlert("Ошибка ввода", "Необходимо ввести название места товаров и ввести номер", "OK");
                return;
            }

            await Navigation.PopAsync();
            var completed = new Place
            {
                Name = Name,
                Order = Order,
                Number = Number,
                ProductTypeId = SelectedProductType?.Id,
                ProductType = SelectedProductType,
                Count = Count,
                Size = Size,
                LastDateTime = LastDateTime,
                PlaceStatus = PlaceStatus,
                Comment = Comment,
                IsDelete = IsDelete,
            };
            ContinueAction.Invoke(completed);
        }

        #endregion

        #region Вспомогательное

        /// <summary> Установка данных на вьюмодель </summary>
        public void SetData(Place place, IEnumerable<ProductType> productTypes)
        {
            Name = place.Name;
            Order = place.Order;
            Number = place.Number;
            ProductTypes.Clear();
            foreach (var productType in productTypes)
                ProductTypes.Add(productType);
            SelectedProductType = ProductTypes.FirstOrDefault(pt => pt.Id == place.ProductTypeId);
            Count = place.Count;
            Size = place.Size;
            LastDateTime = place.LastDateTime;
            PlaceStatus = place.PlaceStatus;
            Comment = place.Comment;
            IsDelete = place.IsDelete;
        }

        #endregion
    }
}
