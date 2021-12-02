﻿using BricksWarehouse.Mobile.ViewModels.Edit;
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


    }
}
