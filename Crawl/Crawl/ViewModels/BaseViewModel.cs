using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using Crawl.Services;
using Crawl.Models;

namespace Crawl.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region RefactorLater

        // Dependency for Mock
        private IDataStore DataStoreMock => DependencyService.Get<IDataStore>() ?? MockDataStore.Instance;

        // Dependency for SQL
        private IDataStore DataStoreSql => DependencyService.Get<IDataStore>() ?? SQLDataStore.Instance;

        public IDataStore DataStore; // Datastore

        // Constructor.
        public BaseViewModel()
        {
            SetDataStore(DataStoreEnum.Mock);
        }

        // Set to SQL or Mock.
        public void SetDataStore(DataStoreEnum data)
        {
            switch (data)
            {
                case DataStoreEnum.Mock:
                    DataStore = DataStoreMock;
                    break;

                case DataStoreEnum.Sql:
                    DataStore = DataStoreSql;
                    break;
                case DataStoreEnum.Unknown:
                default:
                    DataStore = DataStoreSql;
                    break;
            }
        }

        #endregion

        private bool _isBusy = false; // Default is Busy.

        // Check if busy.
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private string _title = string.Empty; // Default title.

        // Get title
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        // Set a property.
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged; // Handle changed property.

        // Update on property changed.
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
