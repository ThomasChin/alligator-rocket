﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Crawl.Models;
using Crawl.Views;
using System.Linq;

namespace Crawl.ViewModels
{
    public class CharactersViewModel : BaseViewModel
    {
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static CharactersViewModel _instance;

        // Instance property
        public static CharactersViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CharactersViewModel();
                }
                return _instance;
            }
        }

        // Collection of Characters to show as a list
        public ObservableCollection<Character> Dataset { get; set; }

        // Load command for loading Character Data
        public Command LoadDataCommand { get; set; }

        // Bool for checking refresh
        private bool _needsRefresh;

        // Constructor.
        public CharactersViewModel()
        {

            Title = "Character List";
            Dataset = new ObservableCollection<Character>();
            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            #region Messages
            MessagingCenter.Subscribe<CharacterDeletePage, Character>(this, "DeleteData", async (obj, data) =>
            {
                await DeleteAsync(data);
            });

            MessagingCenter.Subscribe<CharacterNewPage, Character>(this, "AddData", async (obj, data) =>
            {
                await AddAsync(data);
            });

            MessagingCenter.Subscribe<CharacterEditPage, Character>(this, "EditData", async (obj, data) =>
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

        // Helper for proccing Loading of Character Data
        private async Task ExecuteLoadDataCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Dataset.Clear();
                var dataset = await DataStore.GetAllAsync_Character(true);

                // Example of how to sort the database output using a linq query.
                //Sort the list
                dataset = dataset
                    .OrderBy(a => a.Name)
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

        // Helper function for forcing db refresh
        public void ForceDataRefresh()
        {
            // Reset
            var canExecute = LoadDataCommand.CanExecute(null);
            LoadDataCommand.Execute(null);
        }

        #region DataOperations

        // Create Character db logic
        public async Task<bool> AddAsync(Character data)
        {
            Dataset.Add(data);
            var myReturn = await DataStore.AddAsync_Character(data);
            return myReturn;
        }

        // Delete Character db logic
        public async Task<bool> DeleteAsync(Character data)
        {
            Dataset.Remove(data);
            var myReturn = await DataStore.DeleteAsync_Character(data);
            return myReturn;
        }

        // Update Character db logic
        public async Task<bool> UpdateAsync(Character data)
        {
            // Find the Character, then update it
            var myData = Dataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);
            await DataStore.UpdateAsync_Character(myData);

            _needsRefresh = true;

            return true;
        }

        // Call to database to ensure most recent
        public async Task<Character> GetAsync(string id)
        {
            var myData = await DataStore.GetAsync_Character(id);
            return myData;
        }

        // Having this at the ViewModel, because it has the DataStore
        // That allows the feature to work for both SQL and the MOCk datastores...
        public async Task<bool> InsertUpdateAsync(Character data)
        {
            var myReturn = await DataStore.InsertUpdateAsync_Character(data);
            return myReturn;
        }

        #endregion DataOperations
    }
}
