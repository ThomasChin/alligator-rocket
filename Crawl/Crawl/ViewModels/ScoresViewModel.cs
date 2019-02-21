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
        public Command LoadDataCommand { get; set; }

        private bool _needsRefresh; // Determine if db needs refresh.

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

        // Call to database operation for delete
        public async Task<bool> DeleteAsync(Score data)
        {
            return false;
        }

        // Call to database operation for add
        public async Task<bool> AddAsync(Score data)
        {
            // Implement 
            return false;
        }

        // Call to database operation for update
        public async Task<bool> UpdateAsync(Score data)
        {
            // Implement 
            return false;
        }

        // Call to database to ensure most recent
        public async Task<Score> GetAsync(string id)
        {
            // Implement 
            return null;
        }

        // Return True if a refresh is needed
        // It sets the refresh flag to false
        public bool NeedsRefresh()
        {
            // Implement 
            return false;

        }

        // Sets the need to refresh
        public void SetNeedsRefresh(bool value)
        {
            _needsRefresh = value;
        }

        private async Task ExecuteLoadDataCommand()
        {
            // Implement 
            return;

        }

        public void ForceDataRefresh()
        {
            // Implement 
        }
    }
}