using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace XamarinMvvMSample.iOS.ViewControllers.Base
{
    public abstract class SidePanelController : UINavigationController
    {
        private SidePanelState _state;
        private CGRect _centerPanelRestingFrame;
        private UIViewController _centerPanel;
        private UIViewController _leftPanel;
        private UIViewController _rightPanel;
        private bool _centerPanelHidden;
        private static UIImage defaultImage;

        public UIViewController LeftPanel
        {
            get { return _leftPanel; }
            set
            {
                _leftPanel = value;
            }
        }
        public UIViewController CenterPanel
        {
            get { return _centerPanel; }
            set
            {
                _centerPanel = value;
            }
        }
        public UIViewController RightPanel
        {
            get { return _rightPanel; }
            set
            {
                _rightPanel = value;
            }
        }

        public UIView LeftPanelContainer { get; private set; }

        public UIView RightPanelContainer { get; private set; }

        public UIView CenterPanelContainer { get; private set; }

        public UIViewController VisiblePanel { get; private set; }

        public bool ShouldDelegateAutorotateToVisiblePanel { get; set; }

        public bool CanUnloadLeftPanel { get; set; }

        public bool CanUnloadRightPanel { get; set; }

        public bool ShouldResizeRightPanel { get; set; }

        public bool ShouldResizeLeftPanel { get; set; }


        public bool PushesSidePanels { get; set; }

        public nfloat LeftPanelPercentWidth { get; set; }

        public nfloat LeftPanelFixedWidth { get; set; }

        public nfloat LeftPanelVisibleWidth
        {
            get
            {
                if (CenterPanelHidden && ShouldResizeLeftPanel)
                {
                    return View.Bounds.Width;
                }
                else
                {
                    return LeftPanelFixedWidth != 0 ? LeftPanelFixedWidth : (nfloat)Math.Floor(View.Bounds.Width * LeftPanelPercentWidth);
                }
            }
        }

        public nfloat RightPanelPercentWidth { get; set; }

        public nfloat RightPanelFixedWidth { get; set; }


        public nfloat RightPanelVisibleWidth
        {
            get
            {
                if (CenterPanelHidden && ShouldResizeRightPanel)
                {
                    return View.Bounds.Width;
                }
                else
                {
                    return RightPanelFixedWidth != 0 ? RightPanelFixedWidth : (nfloat)Math.Floor(View.Bounds.Width * RightPanelPercentWidth);
                }
            }
        }

        public nfloat MinimumMovePercentage { get; set; }

        public nfloat MaximumAnimationDuration { get; set; }

        public bool CenterPanelHidden
        {
            get { return _centerPanelHidden; }
            set
            {
                if (CenterPanelHidden != value)
                {
                    SetCenterPanelHidden(value, true, MaximumAnimationDuration);
                }
            }
        }

        public static UIImage DefaultImage
        {
            get
            {
                if (defaultImage == null)
                {
                    UIGraphics.BeginImageContextWithOptions(new CGSize(20f, 13f), false, 0.0f);

                    UIColor.Black.SetFill();
                    UIBezierPath.FromRect(new CGRect(0, 0, 20, 1)).Fill();
                    UIBezierPath.FromRect(new CGRect(0, 5, 20, 1)).Fill();
                    UIBezierPath.FromRect(new CGRect(0, 10, 20, 1)).Fill();

                    UIColor.White.SetFill();
                    UIBezierPath.FromRect(new CGRect(0, 1, 20, 2)).Fill();
                    UIBezierPath.FromRect(new CGRect(0, 6, 20, 2)).Fill();
                    UIBezierPath.FromRect(new CGRect(0, 11, 20, 2)).Fill();

                    defaultImage = UIGraphics.GetImageFromCurrentImageContext();
                    UIGraphics.EndImageContext();
                }
                return defaultImage;
            }
        }

        public SidePanelController()
        {
            Initialize();
        }

        private void Initialize()
        {
            CenterPanelContainer = new UIView();
            _centerPanelHidden = false;
            LeftPanelContainer = new UIView();
            LeftPanelContainer.Hidden = true;
            RightPanelContainer = new UIView();
            RightPanelContainer.Hidden = true;
            State = SidePanelState.CenterVisible;

            LeftPanelPercentWidth = 0.8f;
            RightPanelPercentWidth = 0.8f;
            MinimumMovePercentage = 0.15f;
            MaximumAnimationDuration = 0.2f;

            ShouldDelegateAutorotateToVisiblePanel = true;
        }

        public SidePanelState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    var oldState = _state;
                    _state = value;
                    switch (_state)
                    {
                        case SidePanelState.CenterVisible:
                            VisiblePanel = CenterPanel;
                            LeftPanelContainer.UserInteractionEnabled = false;
                            RightPanelContainer.UserInteractionEnabled = false;
                            break;
                        case SidePanelState.LeftVisible:
                            VisiblePanel = LeftPanel;
                            LeftPanelContainer.UserInteractionEnabled = true;
                            break;
                        case SidePanelState.RightVisible:
                            VisiblePanel = RightPanel;
                            RightPanelContainer.UserInteractionEnabled = true;
                            break;
                    }
                }
            }
        }


        public void ShowLeftPanel(bool animated = true)
        {
            ShowLeftPanel(animated, false);
        }

        public void ShowCenterPanel(bool animated = true)
        {
            if (CenterPanelHidden)
            {
                _centerPanelHidden = false;
                UnhideCenterPanel();
            }
            ShowCenterPanel(animated, false);
        }

        public void ShowRightPanel(bool animated = true)
        {
            ShowRightPanel(animated, false);
        }

        public void ToggleLeftPanel(bool animated = true)
        {
            if (State == SidePanelState.LeftVisible)
            {
                ShowCenterPanel(animated, false);
            }
            else if (State == SidePanelState.CenterVisible)
            {
                ShowLeftPanel(animated, false);
            }
        }

        public void ToggleRightPanel(bool animated = true)
        {
            if (State == SidePanelState.RightVisible)
            {
                ShowCenterPanel(animated, false);
            }
            else if (State == SidePanelState.CenterVisible)
            {
                ShowRightPanel(animated, false);
            }
        }

        public UIBarButtonItem GetLeftButtonForCenterPanel()
        {
            return new UIBarButtonItem(DefaultImage, UIBarButtonItemStyle.Plain, (sender, e) => ToggleLeftPanel());
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;

            CenterPanelContainer.Frame = View.Bounds;
            _centerPanelRestingFrame = CenterPanelContainer.Frame;
            LeftPanelContainer.Frame = View.Bounds;
            RightPanelContainer.Frame = View.Bounds;

            ConfigureContainers();

            View.AddSubview(CenterPanelContainer);
            View.AddSubview(LeftPanelContainer);
            View.AddSubview(RightPanelContainer);

            SwapCenter(null, 0, CenterPanel);
            View.BringSubviewToFront(CenterPanelContainer);

            StyleContainer(CenterPanelContainer, false, 0);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            // ensure correct view dimensions
            LayoutSideContainers(false, 0.0f);
            LayoutSidePanels();
            CenterPanelContainer.Frame = AdjustCenterFrame();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            //Account for possible rotation while view appearing
            AdjustCenterFrame();
        }

        private void ConfigureContainers()
        {
            LeftPanelContainer.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleRightMargin;
            RightPanelContainer.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleHeight;
            CenterPanelContainer.Frame = View.Bounds;
            CenterPanelContainer.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
        }

        public void SetCenterPanelHidden(bool hidden, bool animated = true, double duration = 0.0f)
        {
            if (CenterPanelHidden != hidden && State != SidePanelState.CenterVisible)
            {
                _centerPanelHidden = hidden;
                duration = animated ? duration : 0.0f;
                if (hidden)
                {
                    UIView.AnimateNotify(duration, () =>
                    {
                        CGRect frame = CenterPanelContainer.Frame;
                        frame.X = State == SidePanelState.LeftVisible ? CenterPanelContainer.Frame.Width : -CenterPanelContainer.Frame.Width;
                        CenterPanelContainer.Frame = frame;
                        LayoutSideContainers(false, 0);
                        if (ShouldResizeLeftPanel || ShouldResizeRightPanel)
                        {
                            LayoutSidePanels();
                        }
                    }, finished =>
                    {
                        // need to double check in case the user tapped really fast
                        if (CenterPanelHidden)
                        {
                            HideCenterPanel();
                        }
                    });
                }
                else
                {
                    UnhideCenterPanel();
                    UIView.Animate(duration, () =>
                    {
                        if (State == SidePanelState.LeftVisible)
                        {
                            ShowLeftPanel(false);
                        }
                        else
                        {
                            ShowRightPanel(false);
                        }
                        if (ShouldResizeLeftPanel || ShouldResizeRightPanel)
                        {
                            LayoutSidePanels();
                        }
                    });
                }
            }
        }

        public virtual void StyleContainer(UIView container, bool animate, double duration)
        {
            UIBezierPath shadowPath = UIBezierPath.FromRoundedRect(container.Bounds, 0.0f);
            if (animate)
            {
                CABasicAnimation animation = CABasicAnimation.FromKeyPath("shadowPath");
                animation.SetFrom(container.Layer.ShadowPath);
                animation.SetTo(shadowPath.CGPath);
                animation.Duration = duration;
                container.Layer.AddAnimation(animation, "shadowPath");
            }
            container.Layer.ShadowPath = shadowPath.CGPath;
            container.Layer.ShadowColor = UIColor.Black.CGColor;
            container.Layer.ShadowRadius = 10.0f;
            container.Layer.ShadowOpacity = 0.75f;
            container.ClipsToBounds = false;
        }

        public virtual void StylePanel(UIView panel)
        {
            panel.ClipsToBounds = true;
        }

        private void HideCenterPanel()
        {
            CenterPanelContainer.Hidden = true;
            if (CenterPanel != null && CenterPanel.IsViewLoaded)
            {
                CenterPanel.View.RemoveFromSuperview();
            }
        }

        private void UnhideCenterPanel()
        {
            CenterPanelContainer.Hidden = false;
            if (CenterPanel != null && CenterPanel.IsViewLoaded && CenterPanel.View.Superview == null)
            {
                CenterPanel.View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                CenterPanel.View.Frame = CenterPanelContainer.Bounds;
                StylePanel(CenterPanel.View);
                CenterPanelContainer.AddSubview(CenterPanel.View);
            }
        }

        private void LayoutSideContainers(bool animate, double duration)
        {
            CGRect leftFrame = View.Bounds;
            CGRect rightFrame = View.Bounds;
            if (PushesSidePanels && !CenterPanelHidden)
            {
                leftFrame.X = CenterPanelContainer.Frame.X - LeftPanelVisibleWidth;
                rightFrame.X = CenterPanelContainer.Frame.X + CenterPanelContainer.Frame.Width;
            }
            LeftPanelContainer.Frame = leftFrame;
            RightPanelContainer.Frame = rightFrame;
            StyleContainer(LeftPanelContainer, animate, duration);
            StyleContainer(RightPanelContainer, animate, duration);
        }

        private void LayoutSidePanels()
        {
            if (RightPanel != null && RightPanel.IsViewLoaded)
            {
                CGRect frame = RightPanelContainer.Bounds;
                if (ShouldResizeRightPanel)
                {
                    if (!PushesSidePanels)
                    {
                        frame.X = RightPanelContainer.Bounds.Width - RightPanelVisibleWidth;
                    }
                    frame.Width = RightPanelVisibleWidth;
                }
                RightPanel.View.Frame = frame;
            }
            if (LeftPanel != null && LeftPanel.IsViewLoaded)
            {
                CGRect frame = LeftPanelContainer.Bounds;
                if (ShouldResizeLeftPanel)
                {
                    frame.Width = LeftPanelVisibleWidth;
                }
                LeftPanel.View.Frame = frame;
            }
        }

        private void SwapCenter(UIViewController previous, SidePanelState previousState, UIViewController next)
        {
            if (previous != next)
            {
                if (previous != null)
                {
                    previous.WillMoveToParentViewController(null);
                    previous.View.RemoveFromSuperview();
                    previous.RemoveFromParentViewController();
                }
                if (next != null)
                {
                    LoadCenterPanelWithPreviousState(previousState);
                    AddChildViewController(next);
                    CenterPanelContainer.AddSubview(next.View);
                    next.DidMoveToParentViewController(this);
                }
            }
        }

        private void PlaceButtonForLeftPanel()
        {
            if (LeftPanel != null)
            {
                UIViewController buttonController = CenterPanel;
                UINavigationController nav = buttonController as UINavigationController;
                if (nav != null)
                {
                    if (nav.ViewControllers.Length > 0)
                    {
                        buttonController = nav.ViewControllers[0];
                    }
                }
                if (buttonController != null && buttonController.NavigationItem.LeftBarButtonItem == null)
                {
                    buttonController.NavigationItem.LeftBarButtonItem = GetLeftButtonForCenterPanel();
                }
            }
        }

        private void LoadLeftPanel()
        {
            RightPanelContainer.Hidden = true;
            if (LeftPanelContainer.Hidden && LeftPanel != null)
            {
                if (LeftPanel.View.Superview == null)
                {
                    LayoutSidePanels();
                    LeftPanel.View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                    StylePanel(LeftPanel.View);
                    LeftPanelContainer.AddSubview(LeftPanel.View);
                }

                LeftPanelContainer.Hidden = false;
            }
        }

        private void LoadRightPanel()
        {
            LeftPanelContainer.Hidden = true;
            if (RightPanelContainer.Hidden && RightPanel != null)
            {
                if (RightPanel.View.Superview == null)
                {
                    LayoutSidePanels();
                    RightPanel.View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                    StylePanel(RightPanel.View);
                    RightPanelContainer.AddSubview(RightPanel.View);
                }

                RightPanelContainer.Hidden = false;
            }
        }

        private void UnloadPanels()
        {
            if (CanUnloadLeftPanel && LeftPanel != null && LeftPanel.IsViewLoaded)
            {
                LeftPanel.View.RemoveFromSuperview();
            }
            if (CanUnloadRightPanel && RightPanel != null && RightPanel.IsViewLoaded)
            {
                RightPanel.View.RemoveFromSuperview();
            }
        }


        private void AnimateCenterPanel(bool shouldBounce, UICompletionHandler completion)
        {

            // looks bad if we bounce when the center panel grows
            if (_centerPanelRestingFrame.Width > CenterPanelContainer.Frame.Width)
            {
                shouldBounce = false;
            }

            nfloat duration = 1;//CalculatedDuration();
            UIView.AnimateNotify(duration, 0.0f, UIViewAnimationOptions.CurveLinear | UIViewAnimationOptions.LayoutSubviews, () =>
            {
                CenterPanelContainer.Frame = _centerPanelRestingFrame;
                StyleContainer(CenterPanelContainer, true, duration);
            }, finished =>
            {
                completion?.Invoke(finished);
            });
        }

        private CGRect AdjustCenterFrame()
        {
            CGRect frame = View.Bounds;
            switch (State)
            {
                case SidePanelState.CenterVisible:
                    frame.X = 0.0f;
                    break;
                case SidePanelState.LeftVisible:
                    frame.X = LeftPanelVisibleWidth;
                    break;
                case SidePanelState.RightVisible:
                    frame.X = -RightPanelVisibleWidth;
                    break;
            }
            _centerPanelRestingFrame = frame;
            return _centerPanelRestingFrame;
        }

        private void LoadCenterPanelWithPreviousState(SidePanelState previousState)
        {
            PlaceButtonForLeftPanel();

            _centerPanel.View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            _centerPanel.View.Frame = CenterPanelContainer.Bounds;
            StylePanel(_centerPanel.View);
        }

        private void ShowLeftPanel(bool animated, bool shouldBounce)
        {
            State = SidePanelState.LeftVisible;
            LoadLeftPanel();

            AdjustCenterFrame();

            if (animated)
            {
                AnimateCenterPanel(shouldBounce, null);
            }
            else
            {
                CenterPanelContainer.Frame = _centerPanelRestingFrame;
                StyleContainer(CenterPanelContainer, false, 0.0f);
            }

            ToggleScrollsToTopForCenter(false, true, false);
        }

        private void ShowRightPanel(bool animated, bool shouldBounce)
        {
            State = SidePanelState.RightVisible;
            LoadRightPanel();

            AdjustCenterFrame();

            if (animated)
            {
                AnimateCenterPanel(shouldBounce, null);
            }
            else
            {
                CenterPanelContainer.Frame = _centerPanelRestingFrame;
                StyleContainer(CenterPanelContainer, false, 0.0f);
            }

            ToggleScrollsToTopForCenter(false, false, true);
        }

        private void ShowCenterPanel(bool animated, bool shouldBounce)
        {
            State = SidePanelState.CenterVisible;

            AdjustCenterFrame();

            if (animated)
            {
                AnimateCenterPanel(shouldBounce, finished =>
                {
                    LeftPanelContainer.Hidden = true;
                    RightPanelContainer.Hidden = true;
                    UnloadPanels();
                });
            }
            else
            {
                CenterPanelContainer.Frame = _centerPanelRestingFrame;
                StyleContainer(CenterPanelContainer, false, 0.0f);

                LeftPanelContainer.Hidden = true;
                RightPanelContainer.Hidden = true;
                UnloadPanels();
            }

            ToggleScrollsToTopForCenter(true, false, false);
        }

        private void ToggleScrollsToTopForCenter(bool center, bool left, bool right)
        {
            // iPhone only supports 1 active UIScrollViewController at a time
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            {
                ToggleScrollsToTop(center, CenterPanelContainer);
                ToggleScrollsToTop(left, LeftPanelContainer);
                ToggleScrollsToTop(right, RightPanelContainer);
            }
        }

        private bool ToggleScrollsToTop(bool enabled, UIView view)
        {
            UIScrollView scrollView = view as UIScrollView;
            if (scrollView != null)
            {
                scrollView.ScrollsToTop = enabled;
                return true;
            }
            else
            {
                foreach (UIView subview in view.Subviews)
                {
                    if (ToggleScrollsToTop(enabled, subview))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public enum SidePanelState
    {
        Unknown = 0,

        CenterVisible,
        LeftVisible,
        RightVisible
    }
}
