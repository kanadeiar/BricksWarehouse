using BricksWarehouse.Mobile.ViewModels.Trash;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BricksWarehouse.Mobile.Views.Trash
{
    public partial class TrashPlacePage : ContentPage
    {
        public TrashPlaceViewModel ViewModel { get; set; }

        public TrashPlacePage()
        {
            InitializeComponent();
            ViewModel = App.Services.GetRequiredService<TrashPlaceViewModel>();
            ViewModel.Message += Message;
        }

        private void Message(object sender, string s)
        {
            DisplayAlert("Ошибка", s, "ОК");
        }
    }
}