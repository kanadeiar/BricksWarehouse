using System;
using Xamarin.Forms;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using BricksWarehouse.Mobile.ViewModels;

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

            services.AddSingleton<MainPageViewModel>();

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
