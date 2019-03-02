using System;

using Xamarin.Forms;

namespace Crawl.Views.Battle
{
    public class BattleOverPage : ContentPage
    {
        public BattleOverPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

