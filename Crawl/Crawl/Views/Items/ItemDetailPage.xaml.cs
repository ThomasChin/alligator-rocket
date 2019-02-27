using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Crawl.Models;
using Crawl.ViewModels;

namespace Crawl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private ItemDetailViewModel _viewModel;

        // Item DetailPage with viewModel.
        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        // Constuctor for Page
        public ItemDetailPage()
        {
            InitializeComponent();

            var data = new Item();

            _viewModel = new ItemDetailViewModel(data);
            BindingContext = _viewModel;
        }

        // Open Item edit page
        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemEditPage(_viewModel));
        }

        // Open Item Delete page
        private async void Delete_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemDeletePage(_viewModel));
        }

        // Cancel action and return to last page
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}