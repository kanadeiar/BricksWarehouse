using System;
using Xamarin.Forms;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using BricksWarehouse.Mobile.ViewModels;
using BricksWarehouse.Domain.Models;
using BricksWarehouse.Domain.Dto;
using BricksWarehouse.Domain.Interfaces;
using BricksWarehouse.Domain.Mappers;
using BricksWarehouse.Mobile.Services;
using BricksWarehouse.Mobile.ViewModels.Control;
using BricksWarehouse.Mobile.ViewModels.Edit;
using BricksWarehouse.Mobile.ViewModels.Trash;

namespace BricksWarehouse.Mobile
{
    public partial class App : Application
    {
        private static IServiceProvider __Services;
        private static IServiceCollection GetServices()
        {
            var services = new ServiceCollection();
            InitServices(services);
            return services;
        }
        public static IServiceProvider Services => __Services ??= GetServices().BuildServiceProvider();
        private static void InitServices(IServiceCollection services)
        {
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://brickswarehouse.azurewebsites.net") });

            services.AddScoped<ProductTypeClient>();
            services.AddScoped<PlaceClient>();
            services.AddScoped<TaskClient>();

            services.AddScoped<IMapper<ProductType, ProductTypeDto>, DtoMapperService>();
            services.AddScoped<IMapper<ProductTypeDto, ProductType>, DtoMapperService>();
            services.AddScoped<IMapper<Place, PlaceDto>, DtoMapperService>();
            services.AddScoped<IMapper<PlaceDto, Place>, DtoMapperService>();
            services.AddScoped<IMapper<OutTask, OutTaskDto>, DtoMapperService>();
            services.AddScoped<IMapper<OutTaskDto, OutTask>, DtoMapperService>();

            services.AddSingleton<MobileTaskService>();
            services.AddSingleton<ParseQrService>();

            services.AddSingleton<MainPageViewModel>();

            services.AddSingleton<ProductTypeViewModel>();
            services.AddSingleton<PlaceViewModel>();
            services.AddSingleton<EditProductTypeViewModel>();
            services.AddSingleton<EditPlaceViewModel>();
            services.AddSingleton<TrashProductTypeViewModel>();
            services.AddSingleton<TrashPlaceViewModel>();

            services.AddSingleton<TaskListViewModel>();
            services.AddSingleton<TaskDetailViewModel>();
            services.AddSingleton<StartLoadTaskViewModel>();
            services.AddSingleton<BeginLoadTaskViewModel>();
            services.AddSingleton<StartShippingTaskViewModel>();
        }

        public App()
        {
            InitializeComponent();

            MainPage = new Views.FlyoutPage();
        }
        protected override void OnStart()
        {
        }
        protected override void OnSleep()
        {
        }
        protected override void OnResume()
        {
        }
    }
}
