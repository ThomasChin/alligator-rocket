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
        private CharacterDetailViewModel _viewModel;
        public List<Item> ItemPool { get; set; }

        public List<Item> ItemListUnknown { get; set; }
        public List<Item> ItemListHead { get; set; }
        public List<Item> ItemListNecklass { get; set; }
        public List<Item> ItemListHand { get; set; }
        public List<Item> ItemListOffHand { get; set; }
        public List<Item> ItemListFinger { get; set; }
        public List<Item> ItemListRightFinger { get; set; }
        public List<Item> ItemListLeftFinger { get; set; }
        public List<Item> ItemListFeet { get; set; }

        public CharacterItemsPage (CharacterDetailViewModel viewModel)
		{
            
			InitializeComponent ();
            BindingContext = _viewModel = viewModel;
            ItemPool = BattleViewModel.Instance.BattleEngine.ItemPool;

            SortItems();
        }

        private void SortItems()
        {
            ItemListUnknown = GetItemsByType(ItemLocationEnum.Unknown);
            ItemListHead = GetItemsByType(ItemLocationEnum.Head);
            ItemListNecklass = GetItemsByType(ItemLocationEnum.Necklass);
            ItemListHand = GetItemsByType(ItemLocationEnum.PrimaryHand);
            ItemListOffHand = GetItemsByType(ItemLocationEnum.OffHand);
            ItemListFinger = GetItemsByType(ItemLocationEnum.Finger);
            ItemListRightFinger = GetItemsByType(ItemLocationEnum.LeftFinger);
            ItemListLeftFinger = GetItemsByType(ItemLocationEnum.RightFinger);
            ItemListFeet = GetItemsByType(ItemLocationEnum.Feet);
        }

    private List<Item> GetItemsByType(ItemLocationEnum location)
        {
            List<Item> items = new List<Item>();

            for(int i = 0; i < ItemPool.Count; i++)
            {
                if(ItemPool[i].Location == location)
                {
                    items.Add(ItemPool[i]);
                }
            }

            return items;
        }

        // Redirect to last page (CharacterPage)
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}


