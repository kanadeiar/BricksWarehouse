using Microsoft.Extensions.DependencyInjection;

namespace BricksWarehouse.Mobile.ViewModels
{
    public class ViewModelLocator
    {
        public MainPageViewModel MainPageViewModel => App.Services
            .GetRequiredService<MainPageViewModel>();
    }
}
