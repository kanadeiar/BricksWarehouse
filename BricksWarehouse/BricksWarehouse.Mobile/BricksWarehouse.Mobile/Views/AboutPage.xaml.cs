using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BricksWarehouse.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private async void ButtonToSite_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync(new Uri("https://brickswarehouse.azurewebsites.net/"), BrowserLaunchMode.SystemPreferred);
        }
    }
}