using BricksWarehouse.Mobile.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace BricksWarehouse.Mobile.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPageViewModel ViewModel { get; set; }

        public MainPage()
        {
            InitializeComponent();
            ViewModel = App.Services.GetRequiredService<MainPageViewModel>();
        }
    }
}
