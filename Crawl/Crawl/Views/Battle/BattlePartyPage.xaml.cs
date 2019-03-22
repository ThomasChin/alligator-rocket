﻿using System;
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

        // Hold the Monsters
        BattleMonsterListPage _myModalBattleMonsterListPage;

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

            // Leads to Character Detail Page
            var ItemPage = new ItemDropPage(data);
            //await Navigation.PushAsync(new ItemDropPage());
            await Navigation.PushModalAsync(ItemPage);

            // Manually deselect item.
            PartyListView.SelectedItem = null;
        }

        // Close this page, send a message to the Battle Main page to display the next screen
        async void OnCloseClicked(object sender, EventArgs args)
        {
            BattleViewModel.Instance.BattleEngine.EmptyItemPool();
            await Navigation.PopModalAsync();
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