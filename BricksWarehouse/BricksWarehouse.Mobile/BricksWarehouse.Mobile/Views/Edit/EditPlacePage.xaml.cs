using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.ViewModels.Edit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BricksWarehouse.Mobile.Views.Edit
{
    public partial class EditPlacePage : ContentPage
    {
        public EditPlaceViewModel ViewModel { get; set; }
        public EditPlacePage(string title, Place place, Action<Place> action, IEnumerable<ProductType> productTypes)
        {
            InitializeComponent();
            ViewModel = App.Services.GetRequiredService<EditPlaceViewModel>();
            ViewModel.Title = title;
            ViewModel.ContinueAction = action;
            ViewModel.Navigation = this.Navigation;
            ViewModel.SetData(place, productTypes);
        }

        private async void ButtonCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}