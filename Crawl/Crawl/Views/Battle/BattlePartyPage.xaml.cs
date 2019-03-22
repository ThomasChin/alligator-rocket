using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Crawl.Models;
using Crawl.ViewModels;

//Page to show all the characters in the party
namespace Crawl.Views.Battle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BattlePartyPage : ContentPage
    {
        private CharactersViewModel _viewModel;

        // Hold the Monsters
        BattleMonsterListPage _myModalBattleMonsterListPage;

        public BattlePartyPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CharactersViewModel();
            GetCharacters();
        }

        //Load the characters from the battleViewModel, (only the living ones)
        private void GetCharacters()
        {
            List<Character> CharacterList = BattleViewModel.Instance.SelectedCharacters.ToList<Character>();
            for (int i = 0; i < CharacterList.Count; i++)
            {
                if (CharacterList[i].Alive)
                {
                    _viewModel.AddAsync(CharacterList[i]);
                }
            }
        }

        // Push for selecting Character model from index
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Character;
            if (data == null)
                return;

            var ItemPage = new ItemDropPage(data);
           
            //Open a new item drop page to allow the character to change items
            await Navigation.PushModalAsync(ItemPage);

            // Manually deselect item.
            PartyListView.SelectedItem = null;
        }

        // Close this page, send a message to the Battle Main page to display the next screen
        async void OnCloseClicked(object sender, EventArgs args)
        {
            BattleViewModel.Instance.BattleEngine.EmptyItemPool();
            await Navigation.PopModalAsync();
            //Send a message so that the Main battle page knows to open a new monster page
            MessagingCenter.Send<BattlePartyPage>(this, "PartyPageClosed");
        }

        // Show Monsters
        private async void ShowModalPageMonsterList()
        {
            // When you want to show the modal page, just call this method
            // add the event handler for to listen for the modal popping event:
            Crawl.App.Current.ModalPopping += HandleModalPopping;
            _myModalBattleMonsterListPage = new BattleMonsterListPage();
            await Navigation.PushModalAsync(_myModalBattleMonsterListPage);
        }

        // Helper to handle Modal navigation.
        private void HandleModalPopping(object sender, ModalPoppingEventArgs e)
        {
            if (e.Modal == _myModalBattleMonsterListPage)
            {
                _myModalBattleMonsterListPage = null;
                App.Current.ModalPopping -= HandleModalPopping;
            }
        }

    }
}