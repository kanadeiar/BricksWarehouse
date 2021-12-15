using BricksWarehouse.Mobile.ViewModels.Control;
using BricksWarehouse.Mobile.ViewModels.Edit;
using BricksWarehouse.Mobile.ViewModels.Trash;
using Microsoft.Extensions.DependencyInjection;

namespace BricksWarehouse.Mobile.ViewModels
{
    public class ViewModelLocator
    {
        public MainPageViewModel MainPageViewModel => App.Services
            .GetRequiredService<MainPageViewModel>();

        public ProductTypeViewModel ProductTypeViewModel => App.Services
            .GetRequiredService<ProductTypeViewModel>();
        public PlaceViewModel PlaceViewModel => App.Services
            .GetRequiredService<PlaceViewModel>();
        public EditProductTypeViewModel EditProductTypeViewModel => App.Services
            .GetRequiredService<EditProductTypeViewModel>();
        public EditPlaceViewModel EditPlaceViewModel => App.Services
            .GetRequiredService<EditPlaceViewModel>();
        public TrashProductTypeViewModel TrashProductTypeViewModel => App.Services
            .GetRequiredService<TrashProductTypeViewModel>();
        public TrashPlaceViewModel TrashPlaceViewModel => App.Services
            .GetRequiredService<TrashPlaceViewModel>();

        public TaskListViewModel TaskListViewModel => App.Services
            .GetRequiredService<TaskListViewModel>();
        public TaskDetailViewModel TaskDetailViewModel => App.Services
            .GetRequiredService<TaskDetailViewModel>();
        public StartLoadTaskViewModel StartLoadTaskViewModel => App.Services
            .GetRequiredService<StartLoadTaskViewModel>();
    }
}
