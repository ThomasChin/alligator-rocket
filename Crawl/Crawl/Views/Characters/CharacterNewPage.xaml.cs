using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Crawl.Models;

namespace Crawl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharacterNewPage : ContentPage
    {
        public Character Data { get; set; } // Character

        // Page constructor
        public CharacterNewPage()
        {
            InitializeComponent();

            Data = new Character
            {
                Name = "Narwallace",
                Id = Guid.NewGuid().ToString()
            };

            BindingContext = this;
        }

        // Button for saving character to database
        public async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddData", new Character(Data.Name, Data.Type));
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
                Data.Type = ClassTypeEnum.Unknown;

            if (picker.SelectedIndex == 1)
                Data.Type = ClassTypeEnum.Mage;

            if (picker.SelectedIndex == 2)
                Data.Type = ClassTypeEnum.Knight;

            if (picker.SelectedIndex == 3)
                Data.Type = ClassTypeEnum.Assassin;
        }
    }
}