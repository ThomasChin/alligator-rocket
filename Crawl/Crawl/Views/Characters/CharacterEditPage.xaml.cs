using System;
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

        // Character Data get/set
        public Character Data { get; set; }

        // Screen Page Constructor
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
            MessagingCenter.Send(this, "EditData", Data);

            // removing the old CharacterDetails page, 2 up counting this page
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

            // Add a new character details page, with the new Character data on it
            await Navigation.PushAsync(new CharacterDetailPage(new CharacterDetailViewModel(Data)));

            // Last, remove this page
            Navigation.RemovePage(this);
        }

        // Redirect to CharacterPage
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Reroll Character Stats and return to 
        private async void ReRollButton_Clicked(object sender, EventArgs e)
        {
            Data.RollStats();
            MessagingCenter.Send(this, "EditData", Data);

            Navigation.RemovePage(this);
            await Navigation.PushAsync(new CharacterEditPage(_viewModel));
        }


    }
}
