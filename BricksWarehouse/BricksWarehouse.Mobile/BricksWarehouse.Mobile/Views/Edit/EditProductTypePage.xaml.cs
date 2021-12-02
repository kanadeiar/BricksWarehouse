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
    public partial class EditProductTypePage : ContentPage
    {
        public EditProductTypeViewModel ViewModel { get; set; }
        public EditProductTypePage(string title, ProductType productType, Action<ProductType> action)
        {
            InitializeComponent();
            ViewModel = App.Services.GetRequiredService<EditProductTypeViewModel>();
            ViewModel.Title = title;
            ViewModel.ContinueAction = action;
            ViewModel.Navigation = this.Navigation;
            ViewModel.SetData(productType);
        }

        private async void ButtonCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}