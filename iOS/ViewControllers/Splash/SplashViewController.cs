using UIKit;
using XamarinMvvMSample.iOS.ViewControllers.Base;
using XamarinMvvMSample.ViewModels;

namespace XamarinMvvMSample.iOS.ViewControllers.Splash
{
    public partial class SplashViewController : BaseViewController<SplashViewModel>
    {
        public override bool PresentFullscreen => true;

        public SplashViewController() : base(nameof(SplashViewController), null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}

