using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Crawl.Models;
using Crawl.ViewModels;

namespace Crawl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        // Initialize ItemsViewModel
        private ItemsViewModel _viewModel;

        // Page constructor
        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = ItemsViewModel.Instance;
        }

        // Open detail page when item is selected from Index
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Item;
            if (data == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(data)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        // Open a Create Item Page
        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemNewPage());
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
    }
}