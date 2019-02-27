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

        // Initialize SQL DataStore.
        private SQLDataStore()
        {
            CreateTables();
        }

        // Initialize New Tables.
        public void InitializeDatabaseNewTables()
        {
            // Delete the tables
            DeleteTables();

            // make them again
            CreateTables();

            // Populate them
            InitializeSeedData();

            // Tell View Models they need to refresh
            NotifyViewModelsOfDataChange();
        }

        // Create the Database Tables
        private void CreateTables()
        {
            App.Database.CreateTableAsync<Item>().Wait();
            App.Database.CreateTableAsync<BaseCharacter>().Wait();
            App.Database.CreateTableAsync<BaseMonster>().Wait();
            App.Database.CreateTableAsync<Score>().Wait();
        }

        // Delete the Datbase Tables by dropping them
        private void DeleteTables()
        {
            App.Database.DropTableAsync<Item>().Wait();
            App.Database.DropTableAsync<BaseCharacter>().Wait();
            App.Database.DropTableAsync<BaseMonster>().Wait();
            App.Database.DropTableAsync<Score>().Wait();
        }

        // Tells the View Models to update themselves.
        private void NotifyViewModelsOfDataChange()
        {
            ItemsViewModel.Instance.SetNeedsRefresh(true);
            CharactersViewModel.Instance.SetNeedsRefresh(true);
            MonstersViewModel.Instance.SetNeedsRefresh(true);
            ScoresViewModel.Instance.SetNeedsRefresh(true);
        }

        // Initialize starting SQL data.
        private async void InitializeSeedData()
        {
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
            await AddAsync_Character(new Character("Ike", ClassType.Knight));
            await AddAsync_Character(new Character("Kirby", ClassType.Mage));
            await AddAsync_Character(new Character("Marth", ClassType.Assasin));
            await AddAsync_Character(new Character("Charizardddddd", ClassType.Mage));

            // Default Monsters
            await AddAsync_Monster(new Monster("Bigg Baddy", MonsterType.GiantSquid, 30, 10, 10, 2, 4, 1));
            await AddAsync_Monster(new Monster("Mad Maddy", MonsterType.GiantStarfish, 30, 10, 10, 2, 4, 1));
            await AddAsync_Monster(new Monster("Sad Saddy", MonsterType.GiantWhale, 30, 10, 10, 2, 4, 1));

            // Default Scores
            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "SQL First Score", ScoreTotal = 111 });
            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "SQL Second Score", ScoreTotal = 222 });
            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "SQL Third Score", ScoreTotal = 333 });

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
            // Check to see if the item exist
            var oldData = await GetAsync_Item(data.Id);
            if (oldData == null)
            {
                await AddAsync_Item(data);
                return true;
            }

            // Compare it, if different update in the DB
            var UpdateResult = await UpdateAsync_Item(data);
            if (UpdateResult)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> AddAsync_Item(Item data)
        {
            var result = await App.Database.InsertAsync(data);
            if (result == 1)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync_Item(Item data)
        {
            var result = await App.Database.UpdateAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync_Item(Item data)
        {
            var result = await App.Database.DeleteAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<Item> GetAsync_Item(string id)
        {
            var tempResult = await App.Database.GetAsync<Item>(id);

            var result = new Item(tempResult);

            return result;
        }

        public async Task<IEnumerable<Item>> GetAllAsync_Item(bool forceRefresh = false)
        {
            var tempResult = await App.Database.Table<Item>().ToListAsync();

            var result = new List<Item>();
            foreach (var item in tempResult)
            {
                result.Add(new Item(item));
            }

            return result;
        }
        #endregion Item

        #region Character
        // Character
        // Conver to BaseCharacter and then add it
        public async Task<bool> AddAsync_Character(Character data)
        {
            // Convert Character to CharacterBase before saving to Database
            var dataBase = new BaseCharacter(data);

            var result = await App.Database.InsertAsync(dataBase);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> InsertUpdateAsync_Character(Character data)
        {

            // Check to see if the item exist
            var oldData = await GetAsync_Character(data.Id);
            if (oldData == null)
            {
                await AddAsync_Character(data);
                return true;
            }

            // Compare it, if different update in the DB
            var UpdateResult = await UpdateAsync_Character(data);
            if (UpdateResult)
            {
                await AddAsync_Character(data);
                return true;
            }

            return false;
        }

        // Convert to BaseCharacter and then update it
        public async Task<bool> UpdateAsync_Character(Character data)
        {
            // Convert Character to CharacterBase before saving to Database
            var dataBase = new BaseCharacter(data);

            var result = await App.Database.UpdateAsync(dataBase);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Pass in the character and convert to Character to then delete it
        public async Task<bool> DeleteAsync_Character(Character data)
        {
            // Convert Character to CharacterBase before saving to Database
            var dataBase = new BaseCharacter(data);

            var result = await App.Database.DeleteAsync(dataBase);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Get the Character Base, and Load it back as Character
        public async Task<Character> GetAsync_Character(string id)
        {
            var tempResult = await App.Database.GetAsync<BaseCharacter>(id);

            var result = new Character(tempResult);

            return result;
        }

        // Load each character as the base character, 
        // Then then convert the list to characters to push up to the view model
        public async Task<IEnumerable<Character>> GetAllAsync_Character(bool forceRefresh = false)
        {
            var tempResult = await App.Database.Table<BaseCharacter>().ToListAsync();

            var result = new List<Character>();
            foreach (var item in tempResult)
            {
                result.Add(new Character(item));
            }

            return result;
        }

        #endregion Character

        #region Monster
        //Monster
        // Conver to BaseMonster and then add it
        public async Task<bool> AddAsync_Monster(Monster data)
        {
            // Convert Monster to MonsterBase before saving to Database
            var dataBase = new BaseMonster(data);

            var result = await App.Database.InsertAsync(dataBase);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> InsertUpdateAsync_Monster(Monster data)
        {

            // Check to see if the item exist
            var oldData = await GetAsync_Monster(data.Id);
            if (oldData == null)
            {
                await AddAsync_Monster(data);
                return true;
            }

            // Compare it, if different update in the DB
            var UpdateResult = await UpdateAsync_Monster(data);
            if (UpdateResult)
            {
                await AddAsync_Monster(data);
                return true;
            }

            return false;
        }

        // Convert to BaseMonster and then update it
        public async Task<bool> UpdateAsync_Monster(Monster data)
        {
            // Convert Monster to MonsterBase before saving to Database
            var dataBase = new BaseMonster(data);

            var result = await App.Database.UpdateAsync(dataBase);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Pass in the Monster and convert to Monster to then delete it
        public async Task<bool> DeleteAsync_Monster(Monster data)
        {
            // Convert Monster to MonsterBase before saving to Database
            var dataBase = new BaseMonster(data);

            var result = await App.Database.DeleteAsync(dataBase);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        // Get the Monster Base, and Load it back as Monster
        public async Task<Monster> GetAsync_Monster(string id)
        {
            var tempResult = await App.Database.GetAsync<BaseMonster>(id);

            var result = new Monster(tempResult);

            return result;
        }

        // Load each Monster as the base Monster, 
        // Then then convert the list to Monsters to push up to the view model
        public async Task<IEnumerable<Monster>> GetAllAsync_Monster(bool forceRefresh = false)
        {
            var tempResult = await App.Database.Table<BaseMonster>().ToListAsync();

            var result = new List<Monster>();
            foreach (var item in tempResult)
            {
                result.Add(new Monster(item));
            }

            return result;
        }

        #endregion Monster

        #region Score
        // Score
        public async Task<bool> AddAsync_Score(Score data)
        {
            var result = await App.Database.InsertAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateAsync_Score(Score data)
        {
            var result = await App.Database.UpdateAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync_Score(Score data)
        {
            var result = await App.Database.DeleteAsync(data);
            if (result == 1)
            {
                return true;
            }

            return false;
        }

        public async Task<Score> GetAsync_Score(string id)
        {
            var tempResult = await App.Database.GetAsync<Score>(id);

            var result = new Score(tempResult);

            return result;
        }

        public async Task<IEnumerable<Score>> GetAllAsync_Score(bool forceRefresh = false)
        {
            var tempResult = await App.Database.Table<Score>().ToListAsync();

            var result = new List<Score>();
            foreach (var Score in tempResult)
            {
                result.Add(new Score(Score));
            }

            return result;
        }

        public async Task<bool> InsertUpdateAsync_Score(Score data)
        {

            // Check to see if the item exist
            var oldData = await GetAsync_Score(data.Id);
            if (oldData == null)
            {
                await AddAsync_Score(data);
                return true;
            }

            // Compare it, if different update in the DB
            var UpdateResult = await UpdateAsync_Score(data);
            if (UpdateResult)
            {
                await AddAsync_Score(data);
                return true;
            }

            return false;
        }

        #endregion Score
    }
}
