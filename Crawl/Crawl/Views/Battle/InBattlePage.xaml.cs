using Crawl.Models;
using Crawl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Crawl.Views.Battle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InBattlePage : ContentPage
    {
        // BattleViewModel instance.
        private BattleViewModel _viewModel;

        // Constructor
        public InBattlePage(BattleViewModel _viewModel1)
        {
            InitializeComponent();
            BindingContext = _viewModel = _viewModel1;
        }

        // Move to Battle Over Page for now...
        async void OnNextClicked(object sender, EventArgs args)
        {
            // Jump to Main Battle Page
            await Navigation.PushAsync(new ItemDropPage());

            // Last, remove this page
            Navigation.RemovePage(this);
        }

        // Load character Data when page is opened.
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = null;

            InitializeComponent();

            _viewModel.LoadDataCommand.Execute(null);

            BindingContext = _viewModel;
        }
    }
}
