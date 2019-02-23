using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crawl.Models;
using Crawl.ViewModels;

namespace Crawl.Services
{
    public sealed class MockDataStore : IDataStore
    {

        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static MockDataStore _instance;

        // Creates MockDataStore if doesn't exist
        public static MockDataStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MockDataStore();
                }
                return _instance;
            }
        }

        // List of Item Data
        private List<Item> _itemDataset = new List<Item>();

        // List of Character Data
        private List<Character> _characterDataset = new List<Character>();

        // List of Monster Data
        private List<Monster> _monsterDataset = new List<Monster>();

        // List of Score Data
        private List<Score> _scoreDataset = new List<Score>();

        // Constructor that intializes seed data
        private MockDataStore()
        {
            InitilizeSeedData();
        }

        // Load intial data into Mock
        private async void InitilizeSeedData()
        {
            //Image URLs
            string blueHornURL = "http://www.clipartbest.com/cliparts/4T9/X99/4T9X99rTE.jpeg";
            string silverNarwhalArmorURL = "http://www.clipartbest.com/cliparts/yio/6kj/yio6kjKoT.png";
            string animeGirlURL = "http://www.clipartbest.com/cliparts/ace/XAr/aceXAregi.jpg";
            string engagementRingURL = "http://www.clipartbest.com/cliparts/Kij/XL6/KijXL6RRT.png";
            string laserHornURL = "http://www.clipartbest.com/cliparts/7Ta/6G6/7Ta6G6MGc.jpg";

            //Default Items
            await AddAsync_Item(new Item("Mock Blue 'horn'", "I guess this'll work??", blueHornURL, 1, 1, 10, ItemLocationEnum.PrimaryHand, AttributeEnum.Attack));
            await AddAsync_Item(new Item("Mock Silver Narwhal Armor", "Will this even fit?", silverNarwhalArmorURL, 0, 10, 10, ItemLocationEnum.Head, AttributeEnum.Defense));
            await AddAsync_Item(new Item("Mock Anime Girl", "What would a narwhal do with this...", animeGirlURL, 0, 0, 0, ItemLocationEnum.OffHand, AttributeEnum.Speed));
            await AddAsync_Item(new Item("Mock Engagement Ring", "Narwhals can get married too...", engagementRingURL, 0, 5, 1, ItemLocationEnum.LeftFinger, AttributeEnum.Attack));
            await AddAsync_Item(new Item("Mock Laser Horn", "This is self-explanatory", laserHornURL, 100, 100, 1, ItemLocationEnum.Head, AttributeEnum.Attack));

            // Default Characters
            await AddAsync_Character(new Character("Mock Ike", ClassType.Knight));
            await AddAsync_Character(new Character("Kirby", ClassType.Mage));
            await AddAsync_Character(new Character("Marth", ClassType.Assasin));
            await AddAsync_Character(new Character("Charizard", ClassType.Mage));

            // Default Monsters
            await AddAsync_Monster(new Monster("Mock Big Baddy", MonsterType.GiantSquid, 30000, 10000, 10, 2, 4, 1));

            // Default Scores
            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "Mock  First Score", ScoreTotal = 111 });
            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "Mock  Second Score", ScoreTotal = 222 });
            await AddAsync_Score(new Score { Id = Guid.NewGuid().ToString(), Name = "Mock  Third Score", ScoreTotal = 333 });
        }

        // For now, do nothing
        private void CreateTables()
        {
            // Do nothing...
        }

        // Delete the Datbase Tables by dropping them
        public void DeleteTables()
        {
            _characterDataset.Clear();
            _itemDataset.Clear();
            _monsterDataset.Clear();
            _scoreDataset.Clear();
        }

        // Tells the View Models to update themselves.
        private void NotifyViewModelsOfDataChange()
        {
            ItemsViewModel.Instance.SetNeedsRefresh(true);
            CharactersViewModel.Instance.SetNeedsRefresh(true);
            MonstersViewModel.Instance.SetNeedsRefresh(true);
            ScoresViewModel.Instance.SetNeedsRefresh(true);
        }

        // Refresh tables with intial seed data
        public void InitializeDatabaseNewTables()
        {
            DeleteTables();

            // make them again
            CreateTables();

            // Populate them
            InitilizeSeedData();

            // Tell View Models they need to refresh
            NotifyViewModelsOfDataChange();
        }

        #region Item
        // Item
        public async Task<bool> InsertUpdateAsync_Item(Item data)
        {

            // Check to see if the item exist
            var oldData = await GetAsync_Item(data.Id);
            if (oldData == null)
            {
                _itemDataset.Add(data);
                return true;
            }

            // Compare it, if different update in the DB
            var UpdateResult = await UpdateAsync_Item(data);
            if (UpdateResult)
            {
                await AddAsync_Item(data);
                return true;
            }

            return false;
        }

        // Add Item in db
        public async Task<bool> AddAsync_Item(Item data)
        {
            _itemDataset.Add(data);

            return await Task.FromResult(true);
        }

        // Update Item in db
        public async Task<bool> UpdateAsync_Item(Item data)
        {
            var myData = _itemDataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            return await Task.FromResult(true);
        }

        // Delete Item in db
        public async Task<bool> DeleteAsync_Item(Item data)
        {
            var myData = _itemDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _itemDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        // Get Item in db
        public async Task<Item> GetAsync_Item(string id)
        {
            return await Task.FromResult(_itemDataset.FirstOrDefault(s => s.Id == id));
        }

        // List Items in db
        public async Task<IEnumerable<Item>> GetAllAsync_Item(bool forceRefresh = false)
        {
            return await Task.FromResult(_itemDataset);
        }

        #endregion Item

        #region Character
        // Character
        public async Task<bool> InsertUpdateAsync_Character(Character data)
        {

            // Check to see if the Character exist
            var oldData = await GetAsync_Character(data.Id);
            if (oldData == null)
            {
                _characterDataset.Add(data);
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

        // Add Character to db
        public async Task<bool> AddAsync_Character(Character data)
        {
            _characterDataset.Add(data);

            return await Task.FromResult(true);
        }

        // Add Character to db
        public async Task<bool> UpdateAsync_Character(Character data)
        {
            var myData = _characterDataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            return await Task.FromResult(true);
        }

        // Delete Character to db
        public async Task<bool> DeleteAsync_Character(Character data)
        {
            var myData = _characterDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _characterDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        // Get Character from db
        public async Task<Character> GetAsync_Character(string id)
        {
            return await Task.FromResult(_characterDataset.FirstOrDefault(s => s.Id == id));
        }

        // List Monsters from db
        public async Task<IEnumerable<Character>> GetAllAsync_Character(bool forceRefresh = false)
        {
            return await Task.FromResult(_characterDataset);
        }

        #endregion Character

        #region Monster
        //Monster
        public async Task<bool> InsertUpdateAsync_Monster(Monster data)
        {

            // Check to see if the Monster exist
            var oldData = await GetAsync_Monster(data.Id);
            if (oldData == null)
            {
                _monsterDataset.Add(data);
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

        // Add Monster to db
        public async Task<bool> AddAsync_Monster(Monster data)
        {
            _monsterDataset.Add(data);

            return await Task.FromResult(true);
        }

        // Add Monster to db
        public async Task<bool> UpdateAsync_Monster(Monster data)
        {
            var myData = _monsterDataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            return await Task.FromResult(true);
        }

        // Delete Monster to db
        public async Task<bool> DeleteAsync_Monster(Monster data)
        {
            var myData = _monsterDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _monsterDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        // Get Monster from db
        public async Task<Monster> GetAsync_Monster(string id)
        {
            return await Task.FromResult(_monsterDataset.FirstOrDefault(s => s.Id == id));
        }

        // List Monsters from db
        public async Task<IEnumerable<Monster>> GetAllAsync_Monster(bool forceRefresh = false)
        {
            return await Task.FromResult(_monsterDataset);
        }

        #endregion Monster

        #region Score
        // Score
        public async Task<bool> InsertUpdateAsync_Score(Score data)
        {
            // Check to see if the item exist
            var oldData = await GetAsync_Score(data.Id);
            if (oldData == null)
            {
                _scoreDataset.Add(data);
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

        // Add Score to db.
        public async Task<bool> AddAsync_Score(Score data)
        {
            _scoreDataset.Add(data);
            return await Task.FromResult(true);
        }

        // Update Score in db.
        public async Task<bool> UpdateAsync_Score(Score data)
        {
            var myData = _scoreDataset.FirstOrDefault(arg => arg.Id == data.Id);
            if (myData == null)
            {
                return false;
            }

            myData.Update(data);

            return await Task.FromResult(true);
        }

        // Delete Score from db.
        public async Task<bool> DeleteAsync_Score(Score data)
        {
            var myData = _scoreDataset.FirstOrDefault(arg => arg.Id == data.Id);
            _scoreDataset.Remove(myData);

            return await Task.FromResult(true);
        }

        // Get Score by id from db.
        public async Task<Score> GetAsync_Score(string id)
        {
            return await Task.FromResult(_scoreDataset.FirstOrDefault(s => s.Id == id));
        }

        // Get all scores from db.
        public async Task<IEnumerable<Score>> GetAllAsync_Score(bool forceRefresh = false)
        {
            return await Task.FromResult(_scoreDataset);
        }
        #endregion Score
    }
}
