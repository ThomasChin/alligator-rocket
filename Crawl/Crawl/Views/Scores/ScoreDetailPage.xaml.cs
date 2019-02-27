using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Crawl.Models;
using Crawl.ViewModels;

namespace Crawl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScoreDetailPage : ContentPage
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private ScoreDetailViewModel _viewModel;

        // New Score Detail Page with ViewModel.
        public ScoreDetailPage(ScoreDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        // Create new Score Edit Page.
        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScoreEditPage(_viewModel));
        }

        // Create new Score Delete Page.
        private async void Delete_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScoreDeletePage(_viewModel));
        }
    }
}
