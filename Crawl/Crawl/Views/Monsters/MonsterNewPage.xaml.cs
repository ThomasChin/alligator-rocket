using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Crawl.Models;

namespace Crawl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonsterNewPage : ContentPage
    {
        public Monster Data { get; set; }

        public MonsterNewPage()
        {
            InitializeComponent();

            Data = new Monster
            {
                Name = "Monster name",
                Description = "This is a Monster description.",
                Id = Guid.NewGuid().ToString()
            };

            BindingContext = this;
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
        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;

            if (picker.SelectedIndex == 0)
                Data.type = MonsterType.GiantSquid;

            if (picker.SelectedIndex == 1)
                Data.type = MonsterType.GiantStarfish;

            if (picker.SelectedIndex == 2)
                Data.type = MonsterType.GiantWhale;

        }
    }
}