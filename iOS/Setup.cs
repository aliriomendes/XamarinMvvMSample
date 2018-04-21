using Autofac;
using XamarinMvvMSample.iOS.Services;
using XamarinMvvMSample.Services.Interfaces;

namespace XamarinMvvMSample.iOS
{
    public static class Setup
    {
        public static void Initialize()
        {
            App.Initialize();
            RegisterPlatformServices();
            App.Start();
        }

        private static void RegisterPlatformServices()
        {
            Ioc.Instance.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
        }
    }
}
