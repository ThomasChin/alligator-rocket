using System;

using Crawl.Models;
using Crawl.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Crawl.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MonsterEditPage : ContentPage
	{
        // ReSharper disable once NotAccessedField.Local
        private MonsterDetailViewModel _viewModel;

        // Monster Data get/set
        public Monster Data { get; set; }

        // Screen Page Constructor
        public MonsterEditPage(MonsterDetailViewModel viewModel)
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

            // removing the old MonsterDetails page, 2 up counting this page
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

            // Add a new monster details page, with the new Monster data on it
            await Navigation.PushAsync(new MonsterDetailPage(new MonsterDetailViewModel(Data)));

            // Last, remove this page
            Navigation.RemovePage(this);
        }

        // Redirect to MonsterPage
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Reroll Monster Stats and return to 
        private async void ReRollButton_Clicked(object sender, EventArgs e)
        {
            Data.RollStats();
            MessagingCenter.Send(this, "EditData", Data);

            Navigation.RemovePage(this);
            await Navigation.PushAsync(new MonsterEditPage(_viewModel));
        }
    }
}
