using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BricksWarehouse.Mobile.ViewModels.Control;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BricksWarehouse.Mobile.Views.Control
{
    public partial class StartShippingTaskPage : ContentPage
    {
        public StartShippingTaskViewModel ViewModel { get; set; }
        public StartShippingTaskPage()
        {
            InitializeComponent();
            ViewModel = App.Services.GetRequiredService<StartShippingTaskViewModel>();

            Task.Run(async () =>
            {
                await ViewModel.UpdateDataAsync();
            });
        }
    }
}