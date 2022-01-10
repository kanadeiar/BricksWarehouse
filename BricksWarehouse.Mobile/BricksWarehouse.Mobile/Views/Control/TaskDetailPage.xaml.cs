using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.ViewModels.Control;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BricksWarehouse.Mobile.Views.Control
{
    public partial class TaskDetailPage : ContentPage
    {
        public TaskDetailViewModel ViewModel { get; set; }
        public TaskDetailPage(OutTask task)
        {
            InitializeComponent();
            ViewModel = App.Services.GetRequiredService<TaskDetailViewModel>();
            ViewModel.Navigation = this.Navigation;
            ViewModel.SetData(task);
        }

        private async void ButtonCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}