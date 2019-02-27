using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Crawl.Models;
using Crawl.ViewModels;

namespace Crawl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScoresPage : ContentPage
    {
        // Use Score View Model.
        private ScoresViewModel _viewModel;

        // Constructor
        public ScoresPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = ScoresViewModel.Instance;
        }

        // Push for selecting Score model from index
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Score;
            if (data == null)
                return;

            // Leads to MoScorenster Detail Page
            await Navigation.PushAsync(new ScoreDetailPage(new ScoreDetailViewModel(data)));

            // Manually deselect item.
            ScoresListView.SelectedItem = null;
        }

        // Push to create a new Score
        private async void AddScore_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScoreNewPage());
        }

        // Load data on apperaring in Index.
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
