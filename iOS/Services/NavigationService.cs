using XamarinMvvMSample.iOS.ViewControllers.Base;
using XamarinMvvMSample.iOS.ViewControllers.Login;
using XamarinMvvMSample.iOS.ViewControllers.Splash;
using XamarinMvvMSample.Services.Interfaces;

namespace XamarinMvvMSample.iOS.Services
{
    public class NavigationService : INavigationService
    {
        public void NavigateToLogin()
        {
            var vc = new LoginViewController();
            RootViewController.Instance.SetRootViewController(vc, RootViewController.Panel.Center);
        }

        public void NavigateToSplashscreen()
        {
            var vc = new SplashViewController();
            RootViewController.Instance.SetRootViewController(vc, RootViewController.Panel.Center);
        }
    }
}
