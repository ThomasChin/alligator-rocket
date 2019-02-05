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

        public CharacterNewPage()
        {
            InitializeComponent();

            Data = new Character
            {
                Name = "Doug",
                Age = 20,
                Id = Guid.NewGuid().ToString()
            };

            BindingContext = this;
        }

        public async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddData", Data);
            await Navigation.PopAsync();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;

            if (picker.SelectedIndex == 0)
                Data.Class = new KnightClass();

            if (picker.SelectedIndex == 1)
                Data.Class = new MageClass();

            if (picker.SelectedIndex == 2)
                Data.Class = new AssasinClass();
        }
    }
}