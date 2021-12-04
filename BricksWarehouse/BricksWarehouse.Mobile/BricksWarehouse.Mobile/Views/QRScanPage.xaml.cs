using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;

namespace BricksWarehouse.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRScanPage : ContentPage
    {
        public QRScanPage()
        {
            InitializeComponent();
        }

        private async void ButtonScan_ClickedAsync(object sender, EventArgs e)
        {
            var scanner = new MobileBarcodeScanner();
            scanner.UseCustomOverlay = false;
            scanner.TopText = "Поместите QR-код в видоискатель камеры для его сканирования.";
            scanner.BottomText = "QR-код сканируется автоматически. Постарайтесь избегать теней и бликов. И старайтесь соблюдать расстояние между устройством и кодом в 15 см.";
            var result = await scanner.Scan(new MobileBarcodeScanningOptions { AutoRotate = false });
            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                await DisplayAlert("Код успешно отсканирован и распознан", $"Результат: {result.Text}", "OK");
            }
            else
            {
                await DisplayAlert("Сканирование отменено", "Сканирование отменено", "OK");
            }
        }
    }
}