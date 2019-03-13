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
    public partial class BattleOverPage : ContentPage
    {
        // Constructor
        public BattleOverPage()
        {
            InitializeComponent();
        }

        // Close this page
        async void OnNextClicked(object sender, EventArgs args)
        {
            // Go back a page.
            await Navigation.PopModalAsync();
            await Navigation.PushModalAsync(new BattleVictoryPage());
        }
    }
}
