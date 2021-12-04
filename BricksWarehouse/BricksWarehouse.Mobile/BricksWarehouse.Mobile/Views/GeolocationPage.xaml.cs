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
    public partial class GeolocationPage : ContentPage
    {
        public GeolocationPage()
        {
            InitializeComponent();
        }

        private async void btnLocation_Clicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    LabelLatitude.Text = "Широта: " + location.Latitude.ToString();
                    LabelLongitude.Text = "Долгота: " + location.Longitude.ToString();
                    LabelCoordinates.Text += $"Широта: {location.Latitude} Долгота: {location.Longitude}\n";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Ошибка", fnsEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Ошибка", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }
    }
}