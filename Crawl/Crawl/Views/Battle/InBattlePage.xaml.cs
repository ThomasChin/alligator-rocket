using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Crawl.Views.Battle
{
    public partial class InBattlePage : ContentPage
    {
        public InBattlePage()
        {
            InitializeComponent();
        }

        // Close this page
        async void OnNextClicked(object sender, EventArgs args)
        {

            // Jump to Main Battle Page
            await Navigation.PushAsync(new Battle.BattleOverPage());

            // Last, remove this page
            Navigation.RemovePage(this);
        }
    }
}
