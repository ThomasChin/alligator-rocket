using Crawl.Models;
using Crawl.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Crawl.GameEngine;

namespace Crawl.Views.Battle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDropPage : ContentPage
    {
        // Initialize ItemsViewModel
        private ItemPoolViewModel _viewModel;
   
        private Character Character;

        // Constructor
        public ItemDropPage(Character TargetCharacter)
        {
            InitializeComponent();
            //Not the instance view model, new view model
            BindingContext = _viewModel = new ItemPoolViewModel();
            Character = TargetCharacter;
            _viewModel.CharacterName = "name";
 

            getItems();
            _viewModel.SetNeedsRefresh(true);
        }

    

        // Close this page
        async void OnCloseClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }

        //Swap the items!
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Item;
            if (data == null)
                return;

            
            BattleViewModel.Instance.BattleEngine.ItemPool.Remove(_viewModel.GetItem(data.Id));
            _viewModel.Dataset.Remove(_viewModel.GetItem(data.Id));

            Item oldItem = Character.AddItem(data.Location, data.Id);

            if (oldItem != null)
            {
                BattleViewModel.Instance.BattleEngine.ItemPool.Add(oldItem);
                _viewModel.Dataset.Add(oldItem);
            }

            ItemReorderPage.Title = "Titllee";

            _viewModel.SetNeedsRefresh(true);
            
            //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(data)));
            //BattleViewModel.Instance.BattleEngine.CharacterList().
        }

        // Load Data for Items 
        private void getItems()
        {
            List<Item> ItemList = BattleViewModel.Instance.BattleEngine.ItemPool.ToList<Item>();
            for (int i = 0; i < ItemList.Count; i++)
                _viewModel.AddAsync(ItemList[i]);
          
        }

    }
}
