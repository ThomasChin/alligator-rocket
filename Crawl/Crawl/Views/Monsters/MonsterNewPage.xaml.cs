using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Crawl.Models;
using System.Collections.Generic;

namespace Crawl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonsterNewPage : ContentPage
    {
        public Monster Data { get; set; } // Monster Data
        public List<int> Difficulties { get; set; } // Monster Difficulties
        public List<string> Types { get; set; } // Monster Types

        // Initialize MonstersNewPage.
        public MonsterNewPage()
        {
            InitializeDifficultes();
          
            //Setup lists for both difficulty types and monster types
            //These will be used to populate the pickers in teh XAML
            InitializeComponent();
            InitializeTypes();

            Data = new Monster
            {
                Name = "Monster name",
                Description = "This is a Monster description.",
                Id = Guid.NewGuid().ToString()
            };

            BindingContext = this;
        }

        //Add all of the monster difficulties to the picker
        private void InitializeDifficultes()
        {
            Difficulties = new List<int>();
            for(int i = Monster.MINDIFF; i <= Monster.MAXDIFF; i++)
                Difficulties.Add(i);
        }

        //Add all of the monster difficulties to the picker
        private void InitializeTypes()
        {
            Types = new List<string>();
            foreach (var type in Enum.GetNames(typeof(MonsterTypeEnum)))
                Types.Add(type);
        }

        // Save Monster to db and redirect to Monster Index
        private async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddData", Data);
            await Navigation.PopAsync();
        }

        // Cancel Monster Creation and redirect to Monster Index
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Picker functionality for Monster type selection
        void OnTypePickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;

            if (picker.SelectedIndex == 0)
                Data.Type = MonsterTypeEnum.GiantSquid;

            if (picker.SelectedIndex == 1)
                Data.Type = MonsterTypeEnum.GiantStarfish;

            if (picker.SelectedIndex == 2)
                Data.Type = MonsterTypeEnum.Whale;

        }

        // Picker functionality for Monster difficulty selection
        void OnDifficultyPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            Data.Difficulty = picker.SelectedIndex;
        }
    }
}