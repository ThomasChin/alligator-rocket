using System;

using Crawl.Views;   

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Crawl.Views.Battle;

namespace Crawl.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OpeningPage : ContentPage
	{
        // Constructor.
		public OpeningPage ()
		{
			InitializeComponent ();
		}

        // Go to Auto Battle page.
        private async void AutoBattleButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AutoBattlePage());
        }

        // Go to Manual Battle page.
        private async void ManualBattleButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AutoBattlePage());
        }
    }
}
