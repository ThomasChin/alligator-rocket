using System;
using System.Collections;
using System.Collections.Generic;

using Crawl.GameEngine;
using Crawl.ViewModels;
using Crawl.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Crawl.Views.Battle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BattleMonsterListPage : ContentPage
    {
        // Data list for Monsters in round.
        public List<Monster> Datalist = new List<Monster>();

        // ViewModel for Battle
        private BattleViewModel _viewModel;

        // Constructor
        public BattleMonsterListPage(BattleViewModel _viewModel1)
        {
            InitializeComponent();

            Datalist = BattleViewModel.Instance.BattleEngine.MonsterList;
            BindingContext = Datalist;
            _viewModel = _viewModel1;

            foreach (var data in Datalist)
            {
                var myHP = new Label()
                {
                    Text = "HP : " + data.GetHealthCurrent(),
                    TextColor = Color.Black,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };

                var myLevel = new Label()
                {
                    Text = "Level : " + data.Level,
                    TextColor = Color.Black,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };

                var myName = new Label()
                {
                    Text = data.Name,
                    TextColor = Color.Black,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };

                StackLayout OuterFrame = new StackLayout
                {
                    MinimumWidthRequest = 400,
                    WidthRequest = 400,
                    Padding = 10
                };

                OuterFrame.Children.Add(myName);
                OuterFrame.Children.Add(myLevel);
                OuterFrame.Children.Add(myHP);

                MonsterListFrame.Children.Add(OuterFrame);
            }
        }

        // Close this page
        async void OnNextClicked(object sender, EventArgs args)
        {
            // Jump to Main Battle Page
            await Navigation.PushAsync(new InBattlePage());

            // Last, remove this page
            Navigation.RemovePage(this);
        }
    }
}
