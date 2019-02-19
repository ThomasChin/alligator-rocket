using Crawl.Models;
using Crawl.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Crawl.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemDeletePage : ContentPage
	{
	    // ReSharper disable once NotAccessedField.Local
	    private ItemDetailViewModel _viewModel;

        // Data getter and setter
        public Item Data { get; set; }

        // Constructor
        public ItemDeletePage (ItemDetailViewModel viewModel)
        {
            // Save off the item
            Data = viewModel.Data;
            viewModel.Title = "Delete " + viewModel.Title;

            InitializeComponent();

            // Set the data binding for the page
            BindingContext = _viewModel = viewModel;
        }

        // Delete Item
	    private async void Delete_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "DeleteData", Data);

            // Remove Item Details Page manualy
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

            await Navigation.PopAsync();
        }

        // Cancel Action and return to screen
	    private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}