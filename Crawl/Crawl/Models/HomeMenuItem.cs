using System;
using System.Collections.Generic;
using System.Text;

namespace Crawl.Models
{
    // Enums for Menu Modules
    public enum MenuItemType
    {
        Home,
        About,
        Score,
        Characters,
        Monsters,
        Items
    }

    // Constructor for Home Menu Item.
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
