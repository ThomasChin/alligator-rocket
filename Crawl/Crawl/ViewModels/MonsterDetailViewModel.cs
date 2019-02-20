using Crawl.Models;

namespace Crawl.ViewModels
{
    public class MonsterDetailViewModel : BaseViewModel
    {
        // Data getters and setters
        public Monster Data { get; set; }
        public MonsterDetailViewModel(Monster data = null)
        {
            Title = data?.Name;
            Data = data;
        }
    }
}
