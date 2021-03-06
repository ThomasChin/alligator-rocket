﻿using System;
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
        public List<Monster> Datalist = new List<Monster>();

        public BattleMonsterListPage()
        {
            InitializeComponent();

            Datalist = BattleViewModel.Instance.BattleEngine.MonsterList;
            BindingContext = Datalist;

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

                var myURI = "Troll2.png";

                if (!string.IsNullOrEmpty(data.ImageURI))
                {
                    myURI = data.ImageURI;
                }

                var myImage = new Image()
                {
                    //Source = FileImageSource.FromUri(new Uri(myURI)),
                    Source = myURI,
                    WidthRequest = 50.0,
                    HeightRequest = 50.0
                };

                #region Relative
                //RelativeLayout myInnerBox = new RelativeLayout();

                //myInnerBox.Children.Add(myImage,
                //    Constraint.Constant(0),
                //    Constraint.Constant(0),
                //    Constraint.RelativeToParent((parent) => { return parent.Width; }),
                //    Constraint.RelativeToParent((parent) => { return parent.Height; }));

                //myInnerBox.Children.Add(myLevel,
                //    Constraint.Constant(0),
                //    Constraint.Constant(0),
                //    Constraint.RelativeToParent((parent) => { return parent.Width; }),
                //    Constraint.RelativeToParent((parent) => { return parent.Height; }));

                //myInnerBox.Children.Add(myHP,
                //    Constraint.Constant(0),
                //    Constraint.Constant(0),
                //    Constraint.RelativeToParent((parent) => { return parent.Width; }),
                //    Constraint.RelativeToParent((parent) => { return parent.Height; }));
                #endregion Relative

                StackLayout OuterFrame = new StackLayout
                {
                    MinimumWidthRequest = 400,
                    WidthRequest = 400,
                    Padding = 10
                };

                OuterFrame.Children.Add(myImage);
                OuterFrame.Children.Add(myName);
                OuterFrame.Children.Add(myLevel);
                OuterFrame.Children.Add(myHP);

                MonsterListFrame.Children.Add(OuterFrame);
            }
        }

        // Close this page
        async void OnNextClicked(object sender, EventArgs args)
        {
            // Go back a page.
            await Navigation.PopModalAsync();
        }
    }
}