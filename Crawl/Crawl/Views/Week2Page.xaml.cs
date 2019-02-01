using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Crawl.Views
{
    public partial class Week2Page : ContentPage
    {
        public Week2Page()
        {
            InitializeComponent();
        }
        private async void HistoryButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScoresPage());
        }
        private async void CharacterButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CharactersPage());
        }
        private async void ItemsButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemsPage());
        }
    }
}
