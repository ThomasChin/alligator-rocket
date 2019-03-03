using System;

using Crawl.Views;
using Crawl.Models;
using System.Collections.ObjectModel;

using Crawl.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Crawl.Views.Battle;

namespace Crawl.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OpeningPage : ContentPage
	{
        // ReSharper disable once NotAccessedField.Local
        private CharactersViewModel _viewModel;

        // Collection of Characters to show as a list
        public ObservableCollection<Character> Data { get; set; }

        // Constructor.
        public OpeningPage ()
		{
            Data = _viewModel.Dataset;

			InitializeComponent ();
            BindingContext = _viewModel;
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
