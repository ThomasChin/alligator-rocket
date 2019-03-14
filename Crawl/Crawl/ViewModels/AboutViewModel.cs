using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace Crawl.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public int RandomValueOverride { get; set; }

        // Constructor.
        public AboutViewModel()
        {
            Title = "About";
        }
    }
}