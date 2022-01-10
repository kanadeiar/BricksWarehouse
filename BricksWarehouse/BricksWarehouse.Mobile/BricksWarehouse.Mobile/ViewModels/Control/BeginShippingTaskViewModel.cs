using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.Services;
using BricksWarehouse.Mobile.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using ZXing.Mobile;

namespace BricksWarehouse.Mobile.ViewModels.Control
{
    public class BeginShippingTaskViewModel : ViewModel
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

        private int _ProductTypeNumber;
        /// <summary> Номер вида товаров </summary>
        public int ProductTypeNumber
        {
            get => _ProductTypeNumber;
            set => Set(ref _ProductTypeNumber, value);
        }

        private string _ProductTypeName;
        /// <summary> Название вида товаров </summary>
        public string ProductTypeName
        {
            get => _ProductTypeName;
            set => Set(ref _ProductTypeName, value);
        }

        private int _PlaceNumber;
        /// <summary> Номер места </summary>
        public int PlaceNumber
        {
            get => _PlaceNumber;
            set => Set(ref _PlaceNumber, value);
        }

        private string _PlaceName;
        /// <summary> Название места </summary>
        public string PlaceName
        {
            get => _PlaceName;
            set => Set(ref _PlaceName, value);
        }

        private int _CountPlace;
        /// <summary> Количество товара на месте </summary>
        public int CountPlace
        {
            get => _CountPlace;
            set => Set(ref _CountPlace, value);
        }

        private int _SizePlace;
        /// <summary> Вместимость места </summary>
        public int SizePlace
        {
            get => _SizePlace;
            set => Set(ref _SizePlace, value);
        }

        private string _title = "Отгрузка со склада";
        /// <summary> Заголовок </summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        public BeginShippingTaskViewModel(MobileTaskService mobileTaskService, ParseQrService parseQrService)
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
                    if (await _MobileTaskService.GetPlaceByNumber(number) is { } place)
                    {
                        if (place.ProductTypeId == _MobileTaskService.ProductType.Id)
                            await Application.Current.MainPage.DisplayAlert("Правильно!", $"Вы выбрали место хранения товаров верно, с него можно загружать товары на грузовик {_MobileTaskService.OutTask.TruckNumber}.", "OK");
                        else
                            await Application.Current.MainPage.DisplayAlert("Неправильно!", "Это не то место хранения товаров, с которого нужно загружать, с него НЕ НУЖНО загружать ничего.", "OK");
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
                else if (datas[0] == "SNPTaskO")
                {
                    if (int.TryParse(datas[1], out int res1))
                    {
                        if (_MobileTaskService.OutTask.Number == res1)
                            await Application.Current.MainPage.DisplayAlert("Правильно!", "Это правильное, выполняемое сейчас задание по отгрузке товаров со склада в грузовик.", "OK");
                        else
                            await Application.Current.MainPage.DisplayAlert("Неправильно!", "Это другое задание, не то, которое сейчас выполняется.", "OK");
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


        private ICommand _GoEndShippingTaskCommand;
        /// <summary> Завершение отгрузки товара со склада, как только он выгружен с транспортера </summary>
        public ICommand GoEndShippingTaskCommand => _GoEndShippingTaskCommand ??=
            new Command(OnGoEndShippingTaskCommandExecuted);
        private async void OnGoEndShippingTaskCommandExecuted()
        {
            var newplace = await _MobileTaskService.EndShippingTask(1);
            var newtask = await _MobileTaskService.GetOneOutTask(_MobileTaskService.OutTask.Id);
            if (newplace != null)
            {
                _MobileTaskService.OutTask.Loaded = newtask.Loaded;
                _MobileTaskService.Place.Count = newplace.Count;
                await Application.Current.MainPage.Navigation.PopAsync();
                var viewModel = App.Services.GetRequiredService<StartShippingTaskViewModel>();
                await viewModel.UpdateDataAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Плохо", "Не удалось отгрузить товар с места хранения товаров на грузовик", "OK");
            }
        }

        #endregion

        #region Вспомогательное

        public void UpdateData()
        {
            Number = _MobileTaskService.OutTask.Number;
            Name = _MobileTaskService.OutTask.Name;
            Loaded = _MobileTaskService.OutTask.Loaded;
            Count = _MobileTaskService.OutTask.Count;
            TruckNumber = _MobileTaskService.OutTask.TruckNumber;
            ProductTypeNumber = _MobileTaskService.ProductType?.FormatNumber ?? 0;
            ProductTypeName = _MobileTaskService.ProductType?.Name;
            PlaceNumber = _MobileTaskService.Place.Number;
            PlaceName = _MobileTaskService.Place.Name;
            CountPlace = _MobileTaskService.Place.Count;
            SizePlace = _MobileTaskService.Place.Size;
        }

        #endregion
    }
}
