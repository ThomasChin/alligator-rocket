
using System;
using Crawl.Services;
using Crawl.Controllers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Crawl.ViewModels;
using Crawl.Models;
using System.Collections.Generic;

namespace Crawl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        // Use AboutViewModel.
        private AboutViewModel _aboutViewModel = new AboutViewModel();

        // Initialize AboutPage.
        public AboutPage()
        {
            InitializeComponent();

            BindingContext = _aboutViewModel;

            // Set the flag for Mock on or off...
            UseMockDataSource.IsToggled = (MasterDataStore.GetDataStoreMockFlag() == DataStoreEnum.Mock);
            SetDataSource(UseMockDataSource.IsToggled);

            // Example of how to add an view to an existing set of XAML. 
            // Give the Xaml layout you want to add the data to a good x:Name, so you can access it.  Here "DateRoot" is what I am using.
            var dateLabel = new Label
            {
                Text = DateTime.Now.ToShortDateString(),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontFamily = "Helvetica Neue",
                FontAttributes = FontAttributes.Bold,
                FontSize = 12,
                TextColor = Color.Black,
            };

            // Set debug settings
            //EnableCriticalMissProblems.IsToggled = GameGlobals.EnableCriticalMissProblems;
            //EnableCriticalHitDamage.IsToggled = GameGlobals.EnableCriticalHitDamage;


            var myTestItem = new Item();
            var myTestCharacter = new Character();
            var myTestMonster = new Monster();

            var myOutputItem = myTestItem.FormatOutput();
            var myOutputCharacter = myTestCharacter.FormatOutput();
            var myOutputMonster = myTestMonster.FormatOutput();

        }

        // Set the Data Source between Mock and SQL.
        private void SetDataSource(bool isMock)
        {
            var set = DataStoreEnum.Sql;

            if (isMock)
            {
                set = DataStoreEnum.Mock;
            }

            MasterDataStore.ToggleDataStore(set);
        }

        // Enable Debug Settings.
        private void Enable20asHit_OnToggled(object sender, ToggledEventArgs e)
        {
            //Implement Logic
        }

        // Enable Debug Settings.
        private void Enable1AsCriticalMiss_OnToggled(object sender, ToggledEventArgs e)
        {
            //Implement Logic
        }

        // Enable Debug Settings.
        private void EnableDebugSettings_OnToggled(object sender, ToggledEventArgs e)
        {
            DebugSettingsFrame.IsVisible = (e.Value);
            DatabaseSettingsFrame.IsVisible = (e.Value);
        }

        // Enable Database Settings.
        private void DatabaseSettingsSwitch_OnToggled(object sender, ToggledEventArgs e)
        {
            DatabaseSettingsFrame.IsVisible = (e.Value);
        }

        // Toggle for Mock and SQL db.
        private void UseMockDataSourceSwitch_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.
            SetDataSource(e.Value);
        }

        // Open a Create Item Page
        private async void AutoBattle_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Battle.AutoBattlePage());
        }

        //Manually Create a battle
        private async void ManualBattle_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Battle.OpeningPage());
        }

        // Turn on Critical Misses
        private void EnableCriticalMissProblems_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.
            GameGlobals.EnableCriticalMissProblems = e.Value;
        }

        // Turn on Critical Hit Damage
        private void EnableCriticalHitDamage_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.
            GameGlobals.EnableCriticalHitDamage = e.Value;
        }

        private async void ClearDatabase_Command(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete", "Sure you want to Delete All Data, and start over?", "Yes", "No");
            if (answer)
            {
                // Call to the SQL DataStore and have it clear the tables.
                SQLDataStore.Instance.InitializeDatabaseNewTables();
            }
        }

        // Server Get Items.
        private async void GetItems_Command(object sender, EventArgs e)
        {
            var myOutput = "No Results";
            var myDataList = new List<Item>();

            var answer = await DisplayAlert("Get", "Sure you want to Get Items from the Server?", "Yes", "No");
            if (answer)
            {
                // Call to the Item Service and have it Get the Items
                // The ServerItemValue Code stands for the batch of items to get
                // as the group to request.  1, 2, 3, 100 (All), or if not specified All

                var value = Convert.ToInt32(ServerItemValue.Text);
                myDataList = await ItemsController.Instance.GetItemsFromServer(value);

                if (myDataList != null && myDataList.Count > 0)
                {
                    // Reset the output
                    myOutput = "";

                    foreach (var item in myDataList)
                    {
                        // Add them line by one, use \n to force new line for output display.
                        // Build up the output string by adding formatted Item Output
                        myOutput += item.FormatOutput() + "\n";
                    }
                }

                await DisplayAlert("Returned List", myOutput, "OK");
            }
        }

        // Server Post Items.
        private async void GetItemsPost_Command(object sender, EventArgs e)
        {
            var myOutput = "No Results";
            var myDataList = new List<Item>();

            var number = Convert.ToInt32(ServerItemValue.Text);
            var level = 6;  // Max Value of 6
            var attribute = AttributeEnum.Unknown;  // Any Attribute
            var location = ItemLocationEnum.Unknown;    // Any Location
            var random = true;  // Random between 1 and Level
            var updateDataBase = true;  // Add them to the DB

            // will return shoes value 10 of speed.
            // Example  result = await ItemsController.Instance.GetItemsFromGame(1, 10, AttributeEnum.Speed, ItemLocationEnum.Feet, false, true);
            myDataList = await ItemsController.Instance.GetItemsFromGame(number, level, attribute, location, random, updateDataBase);

            if (myDataList != null && myDataList.Count > 0)
            {
                // Reset the output
                myOutput = "";

                foreach (var item in myDataList)
                {
                    // Add them line by one, use \n to force new line for output display.
                    myOutput += item.FormatOutput() + "\n";
                }
            }

            await DisplayAlert("Returned List", myOutput, "OK");
        }
    }
}
