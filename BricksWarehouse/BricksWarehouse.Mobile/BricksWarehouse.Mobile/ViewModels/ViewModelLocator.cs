using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BricksWarehouse.Mobile.ViewModels
{
    public class ViewModelLocator
    {
        public MainPageViewModel MainPageViewModel => App.Services
            .GetRequiredService<MainPageViewModel>();
    }
}
