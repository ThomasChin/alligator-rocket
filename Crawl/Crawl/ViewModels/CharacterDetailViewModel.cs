using Crawl.Models;

namespace Crawl.ViewModels
{
    public class CharacterDetailViewModel : BaseViewModel
    {
        public Character Data { get; set; } // Character Data

        // Constructor.
        public CharacterDetailViewModel(Character data = null)
        {
            Title = data?.Name;
            Data = data;
        }
    }
}
