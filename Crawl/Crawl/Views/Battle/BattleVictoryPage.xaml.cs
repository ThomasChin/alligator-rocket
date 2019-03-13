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
    public partial class BattleVictoryPage : TabbedPage
    {
        public BattleVictoryPage ()
        {
            InitializeComponent();
        }


        async void OnButtonClicked(object sender, EventArgs args)
        {
            Navigation.PopModalAsync();
        }
    }



}