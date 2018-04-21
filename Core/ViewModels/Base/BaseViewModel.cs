using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Autofac;
using XamarinMvvMSample.Services.Interfaces;

namespace XamarinMvvMSample
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private static INavigationService _navService;
        protected static INavigationService NavigationService => _navService ?? (_navService = Ioc.Container.Resolve<INavigationService>());

        public IDataStore<Item> DataStore => Ioc.Container.Resolve<IDataStore<Item>>() ?? new MockDataStore();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public Task InitializationTask { get; set; }

        public void InitializeViewModel()
        {
            InitializationTask = InitializeAsync();
        }

        protected virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task Appearing()
        {
            return Task.CompletedTask;
        }

        public virtual Task Disappearing()
        {
            return Task.CompletedTask;
        }

        public void Prepare() { }

        public virtual void PrepareSerialized(string serializedObject) { }

        public virtual void Prepare(object dataObject) { }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName]string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
