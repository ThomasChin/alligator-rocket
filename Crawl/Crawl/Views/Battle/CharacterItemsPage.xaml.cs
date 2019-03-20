using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Crawl.Models;
using Crawl.ViewModels;
using Crawl.GameEngine;

namespace Crawl.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CharacterItemsPage : ContentPage
	{
        private ItemsViewModel _viewModel;
        public CharacterDetailViewModel _characterViewModel;

        public List<Item> ItemPool { get; set; }
 

        public CharacterItemsPage (CharacterDetailViewModel viewModel)
		{
            _viewModel = new ItemsViewModel();
			InitializeComponent ();
            BindingContext = _viewModel;
            _characterViewModel = viewModel;

            ItemPool = BattleViewModel.Instance.BattleEngine.ItemPool;
            LoadItems();
        }


        // Load Data for Items 
        private void LoadItems()
        {
            List<Item> ItemList = ItemPool;
            for (int i = 0; i < ItemList.Count; i++)
                _viewModel.AddAsync(ItemList[i]);
        }

        // Open detail page when item is selected from Index
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Item;
            if (data == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(data)));
        }

        // Redirect to last page (CharacterPage)
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}


