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
    public class StartShippingTaskViewModel : ViewModel
    {
        #region Данные

        private readonly MobileTaskService _MobileTaskService;
        private readonly ParseQrService _ParseQrService;

        #endregion

        #region Свойства

        private int _Number;
        /// <summary> Номер задания </summary>
        public int Number
        {
            get => _Number;
            set => Set(ref _Number, value);
        }

        private string _Name;
        /// <summary> Название задания </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private int _Loaded;
        /// <summary> Загружено </summary>
        public int Loaded
        {
            get => _Loaded;
            set => Set(ref _Loaded, value);
        }

        private int _Count;
        /// <summary> Количество </summary>
        public int Count
        {
            get => _Count;
            set => Set(ref _Count, value);
        }

        private string _TruckNumber;
        /// <summary> Гос номер грузовика </summary>
        public string TruckNumber
        {
            get => _TruckNumber;
            set => Set(ref _TruckNumber, value);
        }

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

        private string _title = "Отгрузка со склада";
        /// <summary> Заголовок </summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        public StartShippingTaskViewModel(MobileTaskService mobileTaskService, ParseQrService parseQrService)
        {
            _MobileTaskService = mobileTaskService;
            _ParseQrService = parseQrService;
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
                    if (place != null)
                    {
                        SelectedRecommendedPlace = place;
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Сканирование не удалось", "Вы неправвильно выбрали место хранения товаров, нужно другое, которое есть в списке рекомендуемых мест хранений товаров", "OK");
                        SelectedRecommendedPlace = null;
                    }
                }
                else if (datas[0] == "SNPUnit")
                {
                    if (int.TryParse(datas[1], out int res))
                    {
                        if (await _MobileTaskService.GetProductTypeByNumber(res) is { } pt)
                        {
                            if (pt.Id == _MobileTaskService.ProductType.Id)
                                await Application.Current.MainPage.DisplayAlert("Правильно!", "Вы выбрали товар верно, его можно загружать. Но вы отсканировали упаковку с товаром, а нужно сканировать место хранения товаров.", "OK");
                            else
                                await Application.Current.MainPage.DisplayAlert("Неправильно!", "Это не тот товар, его НЕ НУЖНО загружать. Вы отсканировали упаковку с товаром, а нужно сканировать место хранения товаров.", "OK");
                        }
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Сканирование не удалось", errorQr, "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Сканирование отменено", "Сканирование отменено", "OK");
            }
        }

        private ICommand _BeginShippingTaskCommand;
        /// <summary> Продолжение выполнения задания по отгрузке товара с места хранения в грузовик </summary>
        public ICommand BeginShippingTaskCommand => _BeginShippingTaskCommand ??=
            new Command(OnBeginShippingTaskCommandExecuted, CanBeginShippingTaskCommandExecute);
        private bool CanBeginShippingTaskCommandExecute(object p) => p is Place;
        private async void OnBeginShippingTaskCommandExecuted(object p)
        {
            var selected = p as Place;
            _MobileTaskService.BeginShippingTask(selected);
            await Application.Current.MainPage.Navigation.PushAsync(new BeginShippingTaskPage());
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

        #region Всмомогательное

        public async Task UpdateDataAsync()
        {
            var places = await _MobileTaskService.GetRecommendedShipmentPlaces(_MobileTaskService.ProductType.Id);
            RecommendedPlaces.Clear();
            SelectedRecommendedPlace = null;
            foreach (var p in places)
                RecommendedPlaces.Add(p);
            Number = _MobileTaskService.OutTask.Number;
            Name = _MobileTaskService.OutTask.Name;
            Loaded = _MobileTaskService.OutTask.Loaded;
            Count = _MobileTaskService.OutTask.Count;
            TruckNumber = _MobileTaskService.OutTask.TruckNumber;
        }

        #endregion
    }
}
