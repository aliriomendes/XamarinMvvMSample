using System;
using System.ComponentModel;
using Autofac;
using Foundation;
using UIKit;

namespace XamarinMvvMSample.iOS.ViewControllers.Base
{
    public abstract class BaseViewController<XViewModel> : UIViewController, IiOSView where XViewModel : BaseViewModel
    {
        public virtual bool PresentFullscreen => false;

        public XViewModel _viewModel;
        public XViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = Ioc.Container.Resolve<XViewModel>()); }
            set { _viewModel = value; }
        }

        public object ParameterData { get; set; }

        public BaseViewController(IntPtr handle) : base(handle) { }
        public BaseViewController(string nibName, NSBundle bundle) : base(nibName, bundle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.PropertyChanged -= OnPropertyChanged;
            ViewModel.PropertyChanged += OnPropertyChanged;
            ViewModel.Prepare(ParameterData);
            ViewModel.InitializeViewModel();

            EdgesForExtendedLayout = UIRectEdge.None;
            NavigationController.SetNavigationBarHidden(PresentFullscreen, false);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           
        }
    }
}
