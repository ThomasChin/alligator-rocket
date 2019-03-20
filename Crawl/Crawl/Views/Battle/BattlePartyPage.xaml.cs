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
            List<Character> CharacterList = BattleViewModel.Instance.AvailableCharacters.ToList<Character>();
            for (int i = 0; i < CharacterList.Count; i++)
                _viewModel.AddAsync(CharacterList[i]);
        }

        // Close this page
        async void OnCloseClicked(object sender, EventArgs args)
        {
            Navigation.RemovePage(this);
        }
    }
}