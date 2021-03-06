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
    public class PlaceViewModel : ViewModel
    {
        #region Данные

        private readonly PlaceClient _placeClient;
        private readonly ProductTypeClient _productTypeClient;

        #endregion

        #region Свойства

        public ObservableCollection<Place> Places { get; set; } = new ObservableCollection<Place>();

        private Place _SelectedPlace;
        /// <summary> Выбранное место </summary>
        public Place SelectedPlace
        {
            get => _SelectedPlace;
            set => Set(ref _SelectedPlace, value);
        }

        /// <summary> Виды товаров для выбора </summary>
        public IEnumerable<ProductType> ProductTypes { get; set; }

        private bool _refreshingPlaces;
        /// <summary> Обновление данных </summary>
        public bool RefreshingPlaces
        {
            get => _refreshingPlaces;
            set => Set(ref _refreshingPlaces, value);
        }

        private string _Title = "Редактирование мест";
        /// <summary> Название приложения </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        public PlaceViewModel(PlaceClient placeClient, ProductTypeClient productTypeClient)
        {
            _placeClient = placeClient;
            _productTypeClient = productTypeClient;

            Task.Run(async () =>
            {
                await UpdateDataAsync();
                RefreshingPlaces = false;
            });
        }

        #region Команды

        private ICommand _AddPlaceCommand;
        /// <summary> Создать место хранения товаров </summary>
        public ICommand AddPlaceCommand => _AddPlaceCommand ??= new Command(OnAddPlaceCommandExecuted);
        private async void OnAddPlaceCommandExecuted(object p)
        {
            var newest = new Place
            {
                Id = 0,
                Name = string.Empty,
            };
            await Application.Current.MainPage.Navigation.PushAsync(new EditPlacePage("Создание места", newest, AddProductTypeCompleted, ProductTypes));
        }
        private async void AddProductTypeCompleted(Place adding)
        {
            adding.Id = 0;
            await _placeClient.Add(adding);
            Places.Add(adding);
        }

        private ICommand _EditPlaceCommand;
        /// <summary> Редактировать место хранения товаров </summary>
        public ICommand EditPlaceCommand => _EditPlaceCommand ??= new Command(OnEditPlaceCommandExecuted, CanEditPlaceCommandExecuted);
        private bool CanEditPlaceCommandExecuted(object p) => p is Place;
        private async void OnEditPlaceCommandExecuted(object p)
        {
            var selected = p as Place;
            await Application.Current.MainPage.Navigation.PushAsync(new EditPlacePage("Редактирование места", selected, EditPlaceCompleted, ProductTypes));
        }
        private async void EditPlaceCompleted(Place editing)
        {
            var selected = SelectedPlace;
            selected.Name = editing.Name;
            selected.Order = editing.Order;
            selected.Number = editing.Number;
            selected.ProductTypeId = editing.ProductTypeId;
            selected.ProductType = editing.ProductType;
            selected.Count = editing.Count;
            selected.Size = editing.Size;
            selected.LastDateTime = editing.LastDateTime;
            selected.PlaceStatus = editing.PlaceStatus;
            selected.Comment = editing.Comment;
            selected.IsDelete = editing.IsDelete;
            await _placeClient.Update(selected);
        }

        private ICommand _DeletePlaceCommand;
        /// <summary> Удалить место хранения товаров </summary>
        public ICommand DeletePlaceCommand => _DeletePlaceCommand ??= new Command(OnDeletePlaceCommandExecuted, CanDeletePlaceCommandExecuted);
        private bool CanDeletePlaceCommandExecuted(object p) => p is Place;
        private async void OnDeletePlaceCommandExecuted(object p)
        {
            if (p is Place place)
            {
                await _placeClient.ToTrash(place.Id);
                Places.Remove(place);
                SelectedPlace = null;
            }
        }

        private ICommand _UpdatePlacesCommand;
        /// <summary> Обновить </summary>
        public ICommand UpdatePlacesCommand => _UpdatePlacesCommand ??= new Command(OnUpdatePlacesCommandExecuted);
        private async void OnUpdatePlacesCommandExecuted(object p)
        {
            RefreshingPlaces = true;
            await UpdateDataAsync();
            RefreshingPlaces = false;
        }

        #endregion

        #region Вспомогательные

        public async Task UpdateDataAsync()
        {
            var places = await _placeClient.GetAll();
            Places.Clear();
            foreach (var place in places)
                Places.Add(place);
            ProductTypes = await _productTypeClient.GetAll();
        }

        #endregion
    }
}
