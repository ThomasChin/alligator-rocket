﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Crawl.Models;
using Crawl.Views;
using System.Linq;
using Crawl.Controllers;
using Crawl.GameEngine;
using Crawl.Views.Battle;

namespace Crawl.ViewModels
{
    public class BattleViewModel : BaseViewModel
    {
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static BattleViewModel _instance;

        public static BattleViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BattleViewModel();
                }
                return _instance;
            }
        }

        // Hold a copy of the Battle Engine
        public BattleEngine BattleEngine;

        // The Character List ot interact with
        // Class for the SelectedCharacters
        public ObservableCollection<Character> SelectedCharacters { get; set; }

        // Class for the AvailableCharacters
        public ObservableCollection<Character> AvailableCharacters { get; set; }

        //Class for SelectedMonsters
        public ObservableCollection<Monster> SelectedMonsters { get; set; }


        // Load the Data command
        public Command LoadDataCommand { get; set; }

        // Flag to check if the data needs refreshing
        private bool _needsRefresh;

        public BattleViewModel()
        {
            Title = "Battle";

            SelectedCharacters = new ObservableCollection<Character>();
            AvailableCharacters = new ObservableCollection<Character>();
            SelectedMonsters = new ObservableCollection<Monster>();

            LoadDataCommand = new Command(async () => await ExecuteLoadDataCommand());

            BattleEngine = new BattleEngine();

            // Load Data
            ExecuteLoadDataCommand().GetAwaiter().GetResult();

            MessagingCenter.Subscribe<OpeningPage, Character>(this, "AddSelectedCharacter", async (obj, data) =>
            {
                SelectedListAdd(data);
            });

            MessagingCenter.Subscribe<OpeningPage, Character>(this, "RemoveSelectedCharacter", async (obj, data) =>
            {
                SelectedListRemove(data);
            });
        }

        #region DataOperations
        // Call to database operation for delete
        public bool SelectedListRemove(Character data)
        {
            SelectedCharacters.Remove(data);
            return true;
        }

        // Call to database operation for add
        public bool SelectedListAdd(Character data)
        {
            SelectedCharacters.Add(data);
            return true;
        }

        // Call to database to ensure most recent
        public Character Get(string id)
        {
            var myData = SelectedCharacters.FirstOrDefault(arg => arg.Id == id);
            if (myData == null)
            {
                return null;
            }

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
                // SelectedCharacters, no need to change them.

                // Reload the Character List from the Character View Moel
                AvailableCharacters.Clear();
                var availableCharacters = CharactersViewModel.Instance.Dataset;
                foreach (var data in availableCharacters)
                {
                    AvailableCharacters.Add(data);
                }

                var availableMonsters = MonstersViewModel.Instance.Dataset;
                foreach (var data in availableMonsters)
                {
                    SelectedMonsters.Add(data);
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
    }
}
