#define MOCK_DATA
using System;
using Autofac;
using XamarinMvvMSample.Services.Interfaces;
using XamarinMvvMSample.ViewModels;

namespace XamarinMvvMSample
{
    public class App
    {
        protected static INavigationService NavigationService => Ioc.Container.Resolve<INavigationService>();

        public static string BackendUrl = "http://localhost:5000";

        public static void Initialize()
        {
            RegisterServices();
            RegisterViewModels();
        }

        private static void RegisterViewModels()
        {
            Ioc.Instance.RegisterType<SplashViewModel>().InstancePerDependency();
            Ioc.Instance.RegisterType<LoginViewModel>().InstancePerDependency();
            Ioc.Instance.RegisterType<AboutViewModel>().InstancePerDependency();
            Ioc.Instance.RegisterType<ItemsViewModel>().InstancePerDependency();
            Ioc.Instance.RegisterType<ItemDetailViewModel>().InstancePerDependency();
        }

        private static void RegisterServices()
        {
#if MOCK_DATA
            Ioc.Instance.RegisterType<MockDataStore>().As<IDataStore<Item>>().SingleInstance();
#else
            Ioc.Instance.RegisterType<CloudDataStore>().As<IDataStore<Item>>().SingleInstance();
#endif
        }

        public static void Start()
        {
            NavigationService.NavigateToSplashscreen();
        }
    }
}
