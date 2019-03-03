using Crawl.Models;
using Crawl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Crawl.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OpeningPage : ContentPage
	{
        private BattleViewModel _viewModel;

        public OpeningPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = BattleViewModel.Instance;
        }

        // Close this page
        async void OnNextClicked(object sender, EventArgs args)
        {

            // Jump to Main Battle Page
            await Navigation.PushAsync(new Battle.AutoBattlePage());

            // Last, remove this page
            Navigation.RemovePage(this);
        }


        private async void OnAvailableCharacterItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Character;
            if (data == null)
            {
                return;
            }

            // Manually deselect item.
            AvailableCharactersListView.SelectedItem = null;

            // Don't add more than the party max
            if (_viewModel.SelectedCharacters.Count() < GameGlobals.MaxNumberPartyPlayers)
            {
                MessagingCenter.Send(this, "AddSelectedCharacter", data);
            }

            PartyCountLabel.Text = _viewModel.SelectedCharacters.Count().ToString();
        }

        private async void OnSelectedCharacterItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Character;
            if (data == null)
            {
                return;
            }

            // Manually deselect item.
            SelectedCharactersListView.SelectedItem = null;

            MessagingCenter.Send(this, "RemoveSelectedCharacter", data);

            PartyCountLabel.Text = _viewModel.SelectedCharacters.Count().ToString();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = null;


            InitializeComponent();

            // Clear the Selected Ones, start over.
            _viewModel.SelectedCharacters.Clear();

            // If the Available Character List is empty fill it and then show it
            if (_viewModel.AvailableCharacters.Count == 0)
            {
                _viewModel.LoadDataCommand.Execute(null);
            }
            else if (_viewModel.NeedsRefresh())
            {
                _viewModel.LoadDataCommand.Execute(null);
            }

            BindingContext = _viewModel;

            PartyCountLabel.Text = _viewModel.SelectedCharacters.Count().ToString();

        }
    }
}
