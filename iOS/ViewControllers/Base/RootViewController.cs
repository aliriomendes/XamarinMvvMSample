using System;
using UIKit;

namespace XamarinMvvMSample.iOS.ViewControllers.Base
{
    public partial class RootViewController : SidePanelController
    {
        public enum Panel
        {
            Left,
            Center,
            Right
        }

        public static RootViewController Instance { get; private set; }

        public RootViewController()
        {
            Instance = this;
            //LeftPanel = new MainPageViewController();

            var navigationController = new UINavigationController();
            CenterPanel = navigationController;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

        }

        public void SetRootViewController(UIViewController viewController, Panel panel)
        {
            DismissViewControllerAsync(true);
            switch (panel)
            {
                case Panel.Left:
                    ((UINavigationController)LeftPanel).SetViewControllers(new UIViewController[] { viewController }, false);
                    break;
                case Panel.Center:
                    ((UINavigationController)CenterPanel).SetViewControllers(new UIViewController[] { viewController }, false);
                    break;
                case Panel.Right:
                    break;
            }
        }

        public void PushViewController(UIViewController viewController, Panel panel, bool animated = true)
        {
            DismissViewControllerAsync(true);
            switch (panel)
            {
                case Panel.Left:
                    ((UINavigationController)LeftPanel).PushViewController(viewController, animated);
                    break;
                case Panel.Center:
                    ((UINavigationController)CenterPanel).PushViewController(viewController, animated);
                    break;
                case Panel.Right:
                    break;
            }
        }

        public void PopToRootViewController(Panel panel)
        {
            DismissViewControllerAsync(true);
            switch (panel)
            {
                case Panel.Left:
                    ((UINavigationController)LeftPanel).PopToRootViewController(false);
                    break;
                case Panel.Center:
                    ((UINavigationController)CenterPanel).PopToRootViewController(false);
                    break;
                case Panel.Right:
                    break;
            }
        }

        public void PopToViewController(UIViewController viewController, Panel panel)
        {
            DismissViewControllerAsync(true);
            switch (panel)
            {
                case Panel.Left:
                    ((UINavigationController)LeftPanel).PopToViewController(viewController, false);
                    break;
                case Panel.Center:
                    ((UINavigationController)CenterPanel).PopToViewController(viewController, false);
                    break;
                case Panel.Right:
                    break;
            }
        }
    }
}
