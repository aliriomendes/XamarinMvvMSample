using UIKit;
using XamarinMvvMSample.iOS.ViewControllers.Base;
using XamarinMvvMSample.ViewModels;

namespace XamarinMvvMSample.iOS.ViewControllers.Login
{
    public partial class LoginViewController : BaseViewController<LoginViewModel>
    {
        public override bool PresentFullscreen => true;

        public LoginViewController() : base(nameof(LoginViewController), null)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}

