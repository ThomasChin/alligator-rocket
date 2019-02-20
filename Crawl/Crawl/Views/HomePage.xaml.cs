using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Crawl.Views
{
    public partial class HomePage : ContentPage
    {
        // Home Page Constructor
        public HomePage()
        {
            InitializeComponent();
        }

        // Open About Page
        private async void AboutButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }
    }
}
