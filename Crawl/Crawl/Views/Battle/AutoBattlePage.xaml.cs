using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crawl.GameEngine;
using Crawl.ViewModels;
using Crawl.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Crawl.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AutoBattlePage : ContentPage
	{
        // Initialize AutoBattlePage.
		public AutoBattlePage ()
		{
			InitializeComponent ();
		}

        private async void AutoBattleButton_Command(object sender, EventArgs e)
        {

            // Hold the score property to give to the score navigation later
            Score myScoreObject = new Score();

            // Can create a new battle engine...
            var myEngine = new AutoBattleEngine();


            await Task.Run(async () =>
            {
                var result = myEngine.ExecuteAutoBattle();

                if (result == false)
                {
                    await DisplayAlert("Error", "No Characters Avaialbe", "OK");
                    return;
                }

                if (myEngine.GetRoundCount() < 1)
                {
                    await DisplayAlert("Error", "No Rounds Fought", "OK");
                    return;
                }

                var myResult = myEngine.GetResultsOutput();
                var myScore = myEngine.GetScoreValue();

                var outputString = "Battle Over! Score " + myScore.ToString();

                myScoreObject = myEngine.GetScoreObject();
            });

            await Navigation.PushAsync(new ScoreDetailPage(new ScoreDetailViewModel(myScoreObject)));
        }
    }
}
