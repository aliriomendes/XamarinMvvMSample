using Foundation;
using UIKit;
using XamarinMvvMSample.iOS.ViewControllers.Base;

namespace XamarinMvvMSample.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window { get; set; }
        public SidePanelController SidePanelController { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            if (Window == null) Window = new UIWindow(UIScreen.MainScreen.Bounds);

            SidePanelController = new RootViewController();
            Window.RootViewController = SidePanelController;

            Setup.Initialize();

            Window.MakeKeyAndVisible();
            return true;
        }
    }
}
