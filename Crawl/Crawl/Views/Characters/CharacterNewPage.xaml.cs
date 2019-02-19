using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Crawl.Models;

namespace Crawl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharacterNewPage : ContentPage
    {
        public Character Data { get; set; }

        // Page constructor
        public CharacterNewPage()
        {
            InitializeComponent();

            Data = new Character
            {
                Name = "Doug",
                Id = Guid.NewGuid().ToString()
            };

            BindingContext = this;
        }

        // Button for saving character to database
        public async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddData", new Character(Data.Name, Data.Class));
            await Navigation.PopAsync();
        }

        // Cancel character creation and return to character index
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Picker functionality for Character class selection
        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;

            if (picker.SelectedIndex == 0)
                Data.Class = ClassType.Base;

            if (picker.SelectedIndex == 1)
                Data.Class = ClassType.Mage;

            if (picker.SelectedIndex == 2)
                Data.Class = ClassType.Knight;

            if (picker.SelectedIndex == 3)
                Data.Class = ClassType.Assasin;
        }
    }
}