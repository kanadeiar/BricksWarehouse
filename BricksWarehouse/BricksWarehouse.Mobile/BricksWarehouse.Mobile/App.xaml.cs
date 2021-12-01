using BricksWarehouse.Domain.Dto;
using BricksWarehouse.Domain.Interfaces;
using BricksWarehouse.Domain.Mappers;
using BricksWarehouse.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

            services.AddScoped<IMapper<ProductType, ProductTypeDto>, DtoMapperService>();
            services.AddScoped<IMapper<ProductTypeDto, ProductType>, DtoMapperService>();
            services.AddScoped<IMapper<Place, PlaceDto>, DtoMapperService>();
            services.AddScoped<IMapper<PlaceDto, Place>, DtoMapperService>();

        }
        public App()
        {
            InitializeComponent();

            MainPage = new FlyoutPage();
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
