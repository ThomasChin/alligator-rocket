﻿using System;
using Crawl.Controllers;
using Crawl.Models;
using Crawl.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Crawl.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CharacterEditPage : ContentPage
	{
	    // ReSharper disable once NotAccessedField.Local
	    private CharacterDetailViewModel _viewModel;

        public Character Data { get; set; }

        public CharacterEditPage(CharacterDetailViewModel viewModel)
        {
            // Save off the item
            Data = viewModel.Data;
            viewModel.Title = "Edit " + viewModel.Title;

            InitializeComponent();
            
            // Set the data binding for the page
            BindingContext = _viewModel = viewModel;
        }

        // Save on the Tool bar
        private async void Save_Clicked(object sender, EventArgs e)
        {
            // If the image in teh data box is empty, use the default one..
            if (string.IsNullOrEmpty(Data.ImageURI))
            {
                Data.ImageURI = CharacterController.DefaultImageURI;
            }

            MessagingCenter.Send(this, "EditData", Data);

            // removing the old ItemDetails page, 2 up counting this page
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

            // Add a new items details page, with the new Item data on it
            await Navigation.PushAsync(new CharacterDetailPage(new CharacterDetailViewModel(Data)));

            // Last, remove this page
            Navigation.RemovePage(this);
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ReRollButton_Clicked(object sender, EventArgs e)
        {
            Data.ReRollStats();
            MessagingCenter.Send(this, "EditData", Data);

            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            await Navigation.PushAsync(new CharacterDetailPage(new CharacterDetailViewModel(Data)));
            Navigation.RemovePage(this);
        }

    }
}