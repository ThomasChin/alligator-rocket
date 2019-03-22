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

//A page to display all of the items in the item pool.
//IF the user clicks on an item, it will be swapped with one of their items, 
//which will be put back into the pool. 
namespace Crawl.Views.Battle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDropPage : ContentPage
    {
        // Initialize ItemsViewModel
        private ItemPoolViewModel _viewModel;
    
        // Character for item.
        private Character Character;

        // Constructor
        public ItemDropPage(Character TargetCharacter)
        {
            InitializeComponent();
            //Not the instance view model, new view model
            BindingContext = _viewModel = new ItemPoolViewModel();
            Character = TargetCharacter;
            _viewModel.CharacterName = "name";
 
            //Load the items int othe view
            getItems();
            _viewModel.SetNeedsRefresh(true);
        }

        // Close this page
        async void OnCloseClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }

        //Swap the selected item with the character's item
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Item;
            if (data == null)
                return;

            
            BattleViewModel.Instance.BattleEngine.ItemPool.Remove(_viewModel.GetItem(data.Id));
            _viewModel.Dataset.Remove(_viewModel.GetItem(data.Id));

            Item oldItem = Character.AddItem(data.Location, data.Id);

            //if theres an old item, swap if into the item pool.
            if (oldItem != null)
            {
                BattleViewModel.Instance.BattleEngine.ItemPool.Add(oldItem);
                _viewModel.Dataset.Add(oldItem);
            }

            //need to refresh the view model 
            _viewModel.SetNeedsRefresh(true);
        }

        // Load Data for Items 
        private void getItems()
        {
            //Add all of the items to the list!
            List<Item> ItemList = BattleViewModel.Instance.BattleEngine.ItemPool.ToList<Item>();
            for (int i = 0; i < ItemList.Count; i++)
            {
                _viewModel.AddAsync(ItemList[i]);
            }
        }
    }
}
