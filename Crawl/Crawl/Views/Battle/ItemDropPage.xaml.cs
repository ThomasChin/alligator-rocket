using Crawl.Models;
using Crawl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Crawl.Views.Battle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDropPage : ContentPage
    {
        // Constructor
        public ItemDropPage()
        {
            InitializeComponent();
        }

        // Close this page
        async void OnNextClicked(object sender, EventArgs args)
        {
            // Jump to BattleOver Page
            await Navigation.PushAsync(new BattleOverPage());

            // Last, remove this page
            Navigation.RemovePage(this);
        }
    }
}
