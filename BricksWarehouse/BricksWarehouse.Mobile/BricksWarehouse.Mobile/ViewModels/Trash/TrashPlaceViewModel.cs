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

namespace BricksWarehouse.Mobile.ViewModels.Trash
{
    public class TrashPlaceViewModel : ViewModel
    {
        #region Данные

        private readonly PlaceClient _placeClient;

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

        private bool _refreshingPlaces;
        /// <summary> Обновление данных </summary>
        public bool RefreshingPlaces
        {
            get => _refreshingPlaces;
            set => Set(ref _refreshingPlaces, value);
        }

        private string _Title = "Корзина удаленных";
        /// <summary> Название приложения </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        public TrashPlaceViewModel(PlaceClient placeClient)
        {
            _placeClient = placeClient;

            Task.Run(async () =>
            {
                await UpdateDataAsync();
                RefreshingPlaces = false;
            });
        }

        #region Команды

        private ICommand _RevertPlaceCommand;
        /// <summary> Восстановить место </summary>
        public ICommand RevertPlaceCommand => _RevertPlaceCommand ??= new Command(OnRevertPlaceCommandExecuted, CanRevertPlaceCommandExecuted);
        private bool CanRevertPlaceCommandExecuted(object p) => p is Place;
        private async void OnRevertPlaceCommandExecuted(object p)
        {
            if (p is Place place)
            {
                await _placeClient.FromTrash(place.Id);
                Places.Remove(place);
                SelectedPlace = null;
            }
        }

        private ICommand _DeletePlaceCommand;
        /// <summary> Удалить окончательно место </summary>
        public ICommand DeletePlaceCommand => _DeletePlaceCommand ??= new Command(OnDeletePlaceCommandExecuted, CanDeletePlaceCommandExecuted);
        private bool CanDeletePlaceCommandExecuted(object p) => p is Place;
        private async void OnDeletePlaceCommandExecuted(object p)
        {
            if (p is Place place)
            {
                await _placeClient.Delete(place.Id);
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
            var places = await _placeClient.GetTrashed();
            Places.Clear();
            foreach (var place in places)
                Places.Add(place);
        }

        #endregion
    }
}
