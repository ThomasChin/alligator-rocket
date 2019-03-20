﻿using Crawl.Models;
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
        private ItemsViewModel _viewModel;

        //private CharacterDetailViewModel CharacterViewModel;

        // Constructor
        public ItemDropPage()
        {
            InitializeComponent();
            //Not the instance view model, new view model
            BindingContext = _viewModel = new ItemsViewModel();
            //CharacterViewModel = charmodel;

            getItems();
        }

        // Load Data for Items 
        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = null;

            if (ToolbarItems.Count > 0)
            {
                ToolbarItems.RemoveAt(0);
            }

            InitializeComponent();

            if (_viewModel.Dataset.Count == 0)
            {
                _viewModel.LoadDataCommand.Execute(null);
            }
            else if (_viewModel.NeedsRefresh())
            {
                _viewModel.LoadDataCommand.Execute(null);
            }

            BindingContext = _viewModel;
        }

        // Close this page
        async void OnCloseClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }

        // Open detail page when item is selected from Index
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Item;
            if (data == null)
                return;

            //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(data)));
        }

        // Load Data for Items 
        private void getItems()
        {
            List<Item> ItemList = BattleViewModel.Instance.BattleEngine.ItemPool;
            for (int i = 0; i < ItemList.Count; i++)
                _viewModel.AddAsync(ItemList[i]);
          
        }

    }
}
