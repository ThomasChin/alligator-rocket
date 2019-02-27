using Crawl.Models;

namespace Crawl.ViewModels
{
    // DetailViewModel inherits from BaseViewModel.
    public class ScoreDetailViewModel : BaseViewModel
    {
        public Score Data { get; set; } // Score Data

        // Constructor
        public ScoreDetailViewModel(Score data = null)
        {
            Title = data?.Name;
            Data = data;
        }
    }
}
