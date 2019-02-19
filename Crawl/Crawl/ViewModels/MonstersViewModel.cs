using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Crawl.Models;
using Crawl.Views;
using System.Linq;

namespace Crawl.ViewModels
{
    public class MonstersViewModel : BaseViewModel
    {
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static MonstersViewModel _instance;

        // Instance property
        public static MonstersViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MonstersViewModel();
                }
                return _instance;
            }
        }

        // Collection of Monsters to show as list
        public ObservableCollection<Monster> Dataset { get; set; }

        // Load command for loading Monster Data
        public Command LoadDataCommand { get; set; }

        // Bool for checking refresh
        private bool _needsRefresh;

        public MonstersViewModel()
        {
            Title = "Monster List";
            Dataset = new ObservableCollection<Monster>();
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            #region Messages
            MessagingCenter.Subscribe<MonsterDeletePage, Monster>(this, "DeleteData", async (obj, data) =>
            {
                await DeleteAsync(data);
            });

            MessagingCenter.Subscribe<MonsterNewPage, Monster>(this, "AddData", async (obj, data) =>
            {
                await AddAsync(data);
            });

            MessagingCenter.Subscribe<MonsterEditPage, Monster>(this, "EditData", async (obj, data) =>
            {
                await UpdateAsync(data);
            });

            #endregion Messages
        }

        // Return True if a refresh is needed
        // It sets the refresh flag to false
        public bool NeedsRefresh()
        {
            if (_needsRefresh)
            {
                _needsRefresh = false;
                return true;
            }

            return false;
        }

        // Sets the need to refresh
        public void SetNeedsRefresh(bool value)
        {
            _needsRefresh = value;
        }

        private async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Dataset.Clear();
                var dataset = await DataStore.GetAllAsync_Monster(true);

                // Example of how to sort the database output using a linq query.
                //Sort the list
                dataset = dataset
                    .OrderBy(a => a.Name)
                    .ThenBy(a => a.Age)
                    .ToList();

                // Then load the data structure
                foreach (var data in dataset)
                {
                    Dataset.Add(data);
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            finally
            {
                IsBusy = false;
            }
        }
        
        public void ForceDataRefresh()
        {
            // Reset
            var canExecute = LoadDataCommand.CanExecute(null);
            LoadDataCommand.Execute(null);
        }

        #region DataOperations

        //Asynchronously adds a monster to the data store. 
        public async Task<bool> AddAsync(Monster data)
        {
            Dataset.Add(data);
            var myReturn = await DataStore.AddAsync_Monster(data);
            return myReturn;
        }

        //Asynchronously deletes a monster from the data store. 
        public async Task<bool> DeleteAsync(Monster data)
        {
            Dataset.Remove(data);
            var myReturn = await DataStore.DeleteAsync_Monster(data);
            return myReturn;
        }

        //Asynchronously updates a monster in the data store. 
        public async Task<bool> UpdateAsync(Monster data)
        {
            // Find the Monster, then update it
            var myData = Dataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);
            await DataStore.UpdateAsync_Monster(myData);

            _needsRefresh = true;

            return true;
        }

        // Call to database to ensure most recent
        public async Task<Monster> GetAsync(string id)
        {
            var myData = await DataStore.GetAsync_Monster(id);
            return myData;
        }

        // Having this at the ViewModel, because it has the DataStore
        // That allows the feature to work for both SQL and the MOCk datastores...
        public async Task<bool> InsertUpdateAsync(Monster data)
        {
            var myReturn = await DataStore.InsertUpdateAsync_Monster(data);
            return myReturn;
        }

        #endregion DataOperations

    }
}