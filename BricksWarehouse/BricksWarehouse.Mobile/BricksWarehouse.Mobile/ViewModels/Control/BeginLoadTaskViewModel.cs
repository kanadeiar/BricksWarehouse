using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.Services;
using BricksWarehouse.Mobile.ViewModels.Base;
using BricksWarehouse.Mobile.Views.Control;
using Xamarin.Forms;
using ZXing.Mobile;

namespace BricksWarehouse.Mobile.ViewModels.Control
{
    public class BeginLoadTaskViewModel : ViewModel
    {
        #region Данные

        private readonly MobileTaskService _MobileTaskService;
        private readonly ParseQrService _ParseQrService;

        #endregion

        #region Свойства

        public ObservableCollection<Place> RecommendedPlaces { get; set; } = new ObservableCollection<Place>();

        private Place _SelectedRecommendedPlace;

        /// <summary> Выбранное место хранений товаров </summary>
        public Place SelectedRecommendedPlace
        {
            get => _SelectedRecommendedPlace;
            set => Set(ref _SelectedRecommendedPlace, value);
        }

        private bool _refreshingRecommendedPlaces;
        /// <summary> Обновление данных </summary>
        public bool RefreshingRecommendedPlaces
        {
            get => _refreshingRecommendedPlaces;
            set => Set(ref _refreshingRecommendedPlaces, value);
        }

        private string _title = "Заполнение склада";
        /// <summary> Заголовок </summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        public BeginLoadTaskViewModel(MobileTaskService mobileTaskService, ParseQrService parseQrService)
        {
            _MobileTaskService = mobileTaskService;
            _ParseQrService = parseQrService;

            Task.Run(async () =>
            {
                await UpdateDataAsync();
                RefreshingRecommendedPlaces = false;
            });
        }

        #region Команды

        private ICommand _ScanQrCodeCommand;
        /// <summary> Сканировать QR код </summary>
        public ICommand ScanQrCodeCommand => _ScanQrCodeCommand ??=
            new Command(OnScanQrCodeCommandExecuted);
        private async void OnScanQrCodeCommandExecuted()
        {
            var scanner = new MobileBarcodeScanner();
            scanner.UseCustomOverlay = false;
            scanner.TopText = "Поместите QR-код в видоискатель камеры для его сканирования.";
            scanner.BottomText = "QR-код сканируется автоматически. Постарайтесь избегать теней и бликов. И старайтесь соблюдать расстояние между устройством и кодом в 15 см.";
            var result = await scanner.Scan(new MobileBarcodeScanningOptions { AutoRotate = false });
            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                var (errorQr, datas) = _ParseQrService.Get(TypeQrCode.Place, result.Text);
                if (string.IsNullOrEmpty(errorQr))
                {
                    var number = int.Parse(datas[1]);
                    var place = RecommendedPlaces.FirstOrDefault(p => p.Number == number);
                    SelectedRecommendedPlace = place;
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Сканирование не удалось", errorQr, "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Сканирование отменено", "Сканирование отменено", "OK");
            }
        }

        private ICommand _GoEndLoadTaskCommand;
        /// <summary> Завершение приема товара после того, как он выгружен с транспортера </summary>
        public ICommand GoEndLoadTaskCommand => _GoEndLoadTaskCommand ??=
            new Command(OnGoEndLoadTaskCommandExecuted, CanGoEndLoadTaskCommandExecute);
        private bool CanGoEndLoadTaskCommandExecute(object p) => p is Place;
        private async void OnGoEndLoadTaskCommandExecuted(object p)
        {
            var place = p as Place;
            _MobileTaskService.BeginLoadTask(place);
            var newplace = await _MobileTaskService.EndLoadTask(1);
            if (newplace != null)
            {
                await Application.Current.MainPage.DisplayAlert("Великолепно", $"Товар [{newplace.ProductType?.FormatNumber}] {newplace.ProductType?.Name} успешно загружен на место хранения товаров [{newplace.Number}] {newplace.Name}, заполнение: {newplace.Count} / {newplace.Size}", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Плохо", "Не удалось загрузить товар на место хранения товаров", "OK");
            }
        }

        private ICommand _UpdateRecommendedPlacesCommand;
        /// <summary> Обновить </summary>
        public ICommand UpdateRecommendedPlacesCommand => _UpdateRecommendedPlacesCommand ??= new Command(OnUpdateRecommendedPlacesCommandExecuted);
        private async void OnUpdateRecommendedPlacesCommandExecuted(object p)
        {
            RefreshingRecommendedPlaces = true;
            await UpdateDataAsync();
            RefreshingRecommendedPlaces = false;
        }

        #endregion

        #region Вспомогательное

        public async Task UpdateDataAsync()
        {
            var places = (await _MobileTaskService.GetRecommendedLoadPlaces(_MobileTaskService.ProductType.Id));
            RecommendedPlaces.Clear();
            SelectedRecommendedPlace = null;
            foreach (var p in places)
                RecommendedPlaces.Add(p);
        }

        #endregion
    }
}
