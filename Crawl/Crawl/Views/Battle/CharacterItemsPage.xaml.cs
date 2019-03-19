using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Crawl.Models;
using Crawl.ViewModels;

namespace Crawl.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharacterItemsPage : ContentPage
    {
        private CharacterDetailViewModel _viewModel;

        public CharacterItemsPage(CharacterDetailViewModel viewModel)
        {
            //InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        // Redirect to last page (CharacterPage)
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}