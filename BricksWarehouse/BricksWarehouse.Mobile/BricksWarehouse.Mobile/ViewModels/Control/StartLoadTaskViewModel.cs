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
    public class StartLoadTaskViewModel : ViewModel
    {
        #region Данные

        private readonly MobileTaskService _MobileTaskService;
        private readonly ProductTypeClient _ProductTypeClient;
        private readonly ParseQrService _ParseQrService;

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

        private string _title = "Заполнение склада";
        /// <summary> Заголовок </summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        public StartLoadTaskViewModel(MobileTaskService mobileTaskService, ProductTypeClient productTypeClient, ParseQrService parseQrService)
        {
            _MobileTaskService = mobileTaskService;
            _ProductTypeClient = productTypeClient;
            _ParseQrService = parseQrService;

            Task.Run(async () =>
            {
                await UpdateDataAsync();
                RefreshingProductTypes = false;
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
                var (errorQr, datas) = _ParseQrService.Get(TypeQrCode.ProductType, result.Text);
                if (string.IsNullOrEmpty(errorQr))
                {
                    var number = int.Parse(datas[1]);
                    var pt = ProductTypes.FirstOrDefault(pt => pt.FormatNumber == number);
                    SelectedProductType = pt;
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Сканирование не удалось", errorQr, "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Сканирование отменено", "Сканирование отменено", "OK");
            }
        }

        private ICommand _GoBeginLoadTaskCommand;
        /// <summary> Продолжение приема товара после того, как он загружен на транспортер </summary>
        public ICommand GoBeginLoadTaskCommand => _GoBeginLoadTaskCommand ??=
            new Command(OnGoBeginLoadTaskCommandExecuted, CanGoBeginLoadTaskCommandExecute);
        private bool CanGoBeginLoadTaskCommandExecute(object p) => p is ProductType;
        private async void OnGoBeginLoadTaskCommandExecuted(object p)
        {
            var productType = p as ProductType;
            _MobileTaskService.StartLoadTask(productType);
            await Application.Current.MainPage.Navigation.PushAsync(new BeginLoadTaskPage());
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
            var productTypes = (await _ProductTypeClient.GetAll());
            ProductTypes.Clear();
            SelectedProductType = null;
            foreach (var pt in productTypes.OrderBy(pt => pt.FormatNumber))
                ProductTypes.Add(pt);
        }

        #endregion
    }
}
