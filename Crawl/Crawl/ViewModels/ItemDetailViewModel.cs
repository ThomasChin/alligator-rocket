using Crawl.Models;

namespace Crawl.ViewModels
{
    // ItemDetailViewModel
    public class ItemDetailViewModel : BaseViewModel
    {
        // Item Data (Getter and setter)
        public Item Data { get; set; }

        // Constructor
        public ItemDetailViewModel(Item data = null)
        {
            Title = data?.Name;
            Data = data;
        }
    }
}
