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
        public Monster Data { get; set; }
        public List<int> Difficulties { get; set; }
        public List<string> typeNames;

        public MonsterNewPage()
        {
            InitializeDifficultes();
            InitializeComponent();

            Data = new Monster
            {
                Name = "Monster name",
                Description = "This is a Monster description.",
                Id = Guid.NewGuid().ToString()
            };

            BindingContext = this;

            //Initialize Type picker


            //Initialize Difficulty picker
        }

        //Add all of the monster difficulties to the picker
        private void InitializeDifficultes()
        {
            Difficulties = new List<int>();
            for(int i = Monster.MINDIFF; i <= Monster.MAXDIFF; i++)
                Difficulties.Add(i);
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
                Data.type = MonsterType.GiantSquid;

            if (picker.SelectedIndex == 1)
                Data.type = MonsterType.GiantStarfish;

            if (picker.SelectedIndex == 2)
                Data.type = MonsterType.GiantWhale;

        }

        // Picker functionality for Monster difficulty selection
        void OnDifficultyPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            Data.Difficulty = picker.SelectedIndex;
        }
    }
}