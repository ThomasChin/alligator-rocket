using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Crawl.Models;
using Crawl.Views;
using System.Linq;
using Crawl.Controllers;

namespace Crawl.ViewModels
{
    public class ScoresViewModel : BaseViewModel
    {
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static ScoresViewModel _instance;

        // Instance of viewmodel.
        public static ScoresViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ScoresViewModel();
                }
                return _instance;
            }
        }

        public ObservableCollection<Score> Dataset { get; set; } // Score Dataset
        public Command LoadDataCommand { get; set; } // Load Data

        private bool _needsRefresh; // Determine if db needs refresh.

        // Constructor
        public ScoresViewModel()
        {
            Title = "Score List";
            Dataset = new ObservableCollection<Score>();
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            // Load data from db.
            ExecuteLoadDataCommand().GetAwaiter().GetResult();

            MessagingCenter.Subscribe<ScoreDeletePage, Score>
            (this, "DeleteData", async (obj, data) =>
            {
                await DeleteAsync(data);
            });

            MessagingCenter.Subscribe<ScoreNewPage, Score>
            (this, "AddData", async (obj, data) =>
            {
                await AddAsync(data);
            });

            MessagingCenter.Subscribe<ScoreEditPage, Score>
            (this, "EditData", async (obj, data) =>
            {
                await UpdateAsync(data);
            });
        }

        #region DataOperations

        // Having this at the ViewModel, because it has the DataStore
        // That allows the feature to work for both SQL and the MOCk datastores...
        public async Task<bool> InsertUpdateAsync(Score data)
        {
            var myReturn = await DataStore.InsertUpdateAsync_Score(data);
            return myReturn;
        }

        // Call to database operation for delete
        public async Task<bool> DeleteAsync(Score data)
        {
            Dataset.Remove(data);

            await DataStore.DeleteAsync_Score(data);
            return true;
        }

        // Call to database operation for add
        public async Task<bool> AddAsync(Score data)
        {
            Dataset.Add(data);
            await DataStore.AddAsync_Score(data);
            return true;
        }

        // Call to database operation for update
        public async Task<bool> UpdateAsync(Score data)
        {
            // Find the Score, then update it
            var myData = Dataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            _needsRefresh = true;
            await DataStore.UpdateAsync_Score(data);
            return true;
        }

        // Call to database to ensure most recent
        public async Task<Score> GetAsync(string id)
        {
            var myData = await DataStore.GetAsync_Score(id);
            return myData;
        }
        #endregion DataOperations

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

        // Command that Loads the Data
        private async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            try
            {
                Dataset.Clear();
                var dataset = await DataStore.GetAllAsync_Score(true);
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

        // Force Data to refresh.
        public void ForceDataRefresh()
        {
            var canExecute = LoadDataCommand.CanExecute(null);
            LoadDataCommand.Execute(null);
        }
    }
}
