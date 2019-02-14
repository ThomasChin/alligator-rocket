using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crawl.Models;
using Crawl.ViewModels;

namespace Crawl.Services
{
    public sealed class SQLDataStore : IDataStore
    {

        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static SQLDataStore _instance;

        public static SQLDataStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SQLDataStore();
                }
                return _instance;
            }
        }

        private SQLDataStore()
        {
            // Implement
            // CreateTables();
        }

        // Create the Database Tables
        private void CreateTables()
        {
            // Implement
        }

        // Delete the Datbase Tables by dropping them
        private void DeleteTables()
        {
            // Implement
        }

            // Tells the View Models to update themselves.
            private void NotifyViewModelsOfDataChange()
        {
            // Implement
        }

            public void InitializeDatabaseNewTables()
        {
            // Implement
            
            // Delete the tables

            // make them again

            // Populate them

            // Tell View Models they need to refresh

        }

        private async void InitilizeSeedData()
        {
            // Implement

            await AddAsync_Item(new Item("Blue 'horn'", "I guess this'll work??",
                "http://www.clipartbest.com/cliparts/4T9/X99/4T9X99rTE.jpeg", 1, 1, 10, ItemLocationEnum.PrimaryHand, AttributeEnum.Attack));

            await AddAsync_Item(new Item("Silver Narwhal Armor", "Will this even fit?",
            "http://www.clipartbest.com/cliparts/yio/6kj/yio6kjKoT.png", 0, 10, 10, ItemLocationEnum.Head, AttributeEnum.Defense));

            //Image URLs
            string blueHornURL = "http://www.clipartbest.com/cliparts/4T9/X99/4T9X99rTE.jpeg";
            string silverNarwhalArmorURL = "http://www.clipartbest.com/cliparts/yio/6kj/yio6kjKoT.png";
            string animeGirlURL = "http://www.clipartbest.com/cliparts/ace/XAr/aceXAregi.jpg";
            string engagementRingURL = "http://www.clipartbest.com/cliparts/Kij/XL6/KijXL6RRT.png";
            string laserHornURL = "http://www.clipartbest.com/cliparts/7Ta/6G6/7Ta6G6MGc.jpg";

            //Default Items
            await AddAsync_Item(new Item("Blue 'horn'", "I guess this'll work??", blueHornURL, 1, 1, 10, ItemLocationEnum.PrimaryHand, AttributeEnum.Attack));
            await AddAsync_Item(new Item("Silver Narwhal Armor", "Will this even fit?", silverNarwhalArmorURL, 0, 10, 10, ItemLocationEnum.Head, AttributeEnum.Defense));
            await AddAsync_Item(new Item("Anime Girl", "What would a narwhal do with this...", animeGirlURL, 0, 0, 0, ItemLocationEnum.OffHand, AttributeEnum.Speed));
            await AddAsync_Item(new Item("Engagement Ring", "Narwhals can get married too...", engagementRingURL, 0, 5, 1, ItemLocationEnum.LeftFinger, AttributeEnum.Attack));
            await AddAsync_Item(new Item("Laser Horn", "This is self-explanatory", laserHornURL, 100, 100, 1, ItemLocationEnum.Head, AttributeEnum.Attack));

            // Default Characters
            await AddAsync_Character(new Character("Ike", new KnightClass()));
            await AddAsync_Character(new Character("Kirby", new KnightClass()));
            await AddAsync_Character(new Character("Marth", new AssasinClass()));
            await AddAsync_Character(new Character("Charizard", new MageClass()));

            // Default Monsters
            await AddAsync_Monster(new Monster("Big Baddy", MonsterType.GiantSquid, 30, 10, 10, 2, 4, 1));
            await AddAsync_Monster(new Monster("Mad Maddy", MonsterType.GiantStarfish, 30, 10, 10, 2, 4, 1));
            await AddAsync_Monster(new Monster("Sad Saddy", MonsterType.GiantWhale, 30, 10, 10, 2, 4, 1));
            // Implement Scores

        }

        #region Item
        // Item

        // Add InsertUpdateAsync_Item Method

        // Check to see if the item exists
        // Add your code here.

        // If it does not exist, then Insert it into the DB
        // Add your code here.
        // return true;

        // If it does exist, Update it into the DB
        // Add your code here
        // return true;

        // If you got to here then return false;

        public async Task<bool> InsertUpdateAsync_Item(Item data)
        {
            // Implement

            return false;
        }

        public async Task<bool> AddAsync_Item(Item data)
        {
            var result = await App.Database.InsertAsync(data); if (result == 1) { return true; }
            return false;
        }

        public async Task<bool> UpdateAsync_Item(Item data)
        {
            var result = await App.Database.UpdateAsync(data); if (result == 1) { return true; }
            return false;
        }

        public async Task<bool> DeleteAsync_Item(Item data)
        {
            var result = await App.Database.DeleteAsync(data); if (result == 1) { return true; }
            return false;
        }

        public async Task<Item> GetAsync_Item(string id)
        {
            var result = await App.Database.GetAsync<Item>(id);
            return result;
        }

        public async Task<IEnumerable<Item>> GetAllAsync_Item(bool forceRefresh = false)
        {
            var result = await App.Database.Table<Item>().ToListAsync();
            return result;
    
        }
        #endregion Item

        #region Character
        // Character

        public async Task<bool> InsertUpdateAsync_Character(Character data)
        {
            // Implement

            return false;
        }

        // Conver to BaseCharacter and then add it
        public async Task<bool> AddAsync_Character(Character data)
        {
            var result = await App.Database.InsertAsync(data); if (result == 1) { return true; }
            return false;
        }

        // Convert to BaseCharacter and then update it
        public async Task<bool> UpdateAsync_Character(Character data)
        {
            var result = await App.Database.UpdateAsync(data); if (result == 1) { return true; }
            return false;
        }

        // Pass in the character and convert to Character to then delete it
        public async Task<bool> DeleteAsync_Character(Character data)
        {
            var result = await App.Database.DeleteAsync(data); if (result == 1) { return true; }
            return false;
        }

        // Get the Character Base, and Load it back as Character
        public async Task<Character> GetAsync_Character(string id)
        {
            var result = await App.Database.GetAsync<Character>(id);
            return result;
        }

        // Load each character as the base character, 
        // Then then convert the list to characters to push up to the view model
        public async Task<IEnumerable<Character>> GetAllAsync_Character(bool forceRefresh = false)
        {
            var result = await App.Database.Table<Character>().ToListAsync();
            return result;
        }

        #endregion Character

        #region Monster
        //Monster
        public async Task<bool> InsertUpdateAsync_Monster(Monster data)
        {
            // Implement

            return false;
        }
        public async Task<bool> AddAsync_Monster(Monster data)
        {
            var result = await App.Database.InsertAsync(data); if (result == 1) { return true; }
            return false;
        }

        public async Task<bool> UpdateAsync_Monster(Monster data)
        {
            var result = await App.Database.UpdateAsync(data); if (result == 1) { return true; }
            return false;
        }

        public async Task<bool> DeleteAsync_Monster(Monster data)
        {
            var result = await App.Database.DeleteAsync(data); if (result == 1) { return true; }
            return false;
        }

        public async Task<Monster> GetAsync_Monster(string id)
        {
            var result = await App.Database.GetAsync<Monster>(id);
            return result;
        }

        public async Task<IEnumerable<Monster>> GetAllAsync_Monster(bool forceRefresh = false)
        {
            var result = await App.Database.Table<Monster>().ToListAsync();
            return result;
        }

        #endregion Monster

        #region Score
        // Score
        public async Task<bool> AddAsync_Score(Score data)
        {
            var result = await App.Database.InsertAsync(data); if (result == 1) { return true; }
            return false;
        }

        public async Task<bool> UpdateAsync_Score(Score data)
        {
            var result = await App.Database.UpdateAsync(data); if (result == 1) { return true; }
            return false;
        }

        public async Task<bool> DeleteAsync_Score(Score data)
        {
            var result = await App.Database.DeleteAsync(data); if (result == 1) { return true; }
            return false;
        }

        public async Task<Score> GetAsync_Score(string id)
        {
            var result = await App.Database.GetAsync<Score>(id);
            return result;
        }

        public async Task<IEnumerable<Score>> GetAllAsync_Score(bool forceRefresh = false)
        {
            var result = await App.Database.Table<Score>().ToListAsync();
            return result;
        }

    #endregion Score
    }
}