using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Crawl.Models;
using Crawl.ViewModels;

namespace Crawl.Views.Battle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BattlePartyPage : ContentPage
    {
        private CharactersViewModel _viewModel;

        public BattlePartyPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CharactersViewModel();
            GetCharacters();
        }

        private void GetCharacters()
        {
            List<Character> CharacterList = BattleViewModel.Instance.SelectedCharacters.ToList<Character>();
            for (int i = 0; i < CharacterList.Count; i++)
                _viewModel.AddAsync(CharacterList[i]);
        }

        // Push for selecting Character model from index
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Character;
            if (data == null)
                return;

            // Leads to Character Detail Page
            var ItemPage = new ItemDropPage();
            //await Navigation.PushAsync(new ItemDropPage());
            await Navigation.PushModalAsync(ItemPage);

            // Manually deselect item.
            PartyListView.SelectedItem = null;
        }

        // Close this page
        async void OnCloseClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
    }
}