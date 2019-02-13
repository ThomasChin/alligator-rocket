﻿using System;
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
        private void InitilizeSeedData()
        {

            // Implement

            // Default Items
            _itemDataset.Add(new Item("Blue 'horn'", "I guess this'll work??",
                "http://www.clipartbest.com/cliparts/4T9/X99/4T9X99rTE.jpeg", 1, 1, 10, ItemLocationEnum.PrimaryHand, AttributeEnum.Attack));

            _itemDataset.Add(new Item("Silver Narwhal Armor", "Will this even fit?",
                "http://www.clipartbest.com/cliparts/yio/6kj/yio6kjKoT.png", 0, 10, 10, ItemLocationEnum.Head, AttributeEnum.Defense));

            _itemDataset.Add(new Item("Anime Girl", "What would a narwhal do with this...",
                "http://www.clipartbest.com/cliparts/ace/XAr/aceXAregi.jpg", 0, 0, 0, ItemLocationEnum.OffHand, AttributeEnum.Speed));

            _itemDataset.Add(new Item("Engagement Ring", "Narwhals can get married too...",
                "http://www.clipartbest.com/cliparts/Kij/XL6/KijXL6RRT.png", 0, 5, 1, ItemLocationEnum.LeftFinger, AttributeEnum.Attack));

            _itemDataset.Add(new Item("Laser Horn", "This is self-explanatory",
                "http://www.clipartbest.com/cliparts/7Ta/6G6/7Ta6G6MGc.jpg", 100, 100, 1, ItemLocationEnum.Head, AttributeEnum.Attack));

            // Default Characters
            _characterDataset.Add(new Character("Thomas", new KnightClass()));
            _characterDataset.Add(new Character("George", new KnightClass()));
            _characterDataset.Add(new Character("Laura", new AssasinClass()));
            _characterDataset.Add(new Character("Char", new MageClass()));

            // Default Monsters
            _monsterDataset.Add(new Monster("Baddy", MonsterType.GiantSquid, 30, 10, 10, 2, 4, 1));
            _monsterDataset.Add(new Monster("aBaddy", MonsterType.GiantSquid, 30, 10, 10, 2, 4, 1));
            // Implement Scores
        }

        // For now, do nothing
        private void CreateTables()
        {
            // Do nothing...
        }

        // Delete the Datbase Tables by dropping them
        public void DeleteTables()
        {
            // Implement
        }

        // Tells the View Models to update themselves.
        private void NotifyViewModelsOfDataChange()
        {
            ItemsViewModel.Instance.SetNeedsRefresh(true);
            CharactersViewModel.Instance.SetNeedsRefresh(true);
            MonstersViewModel.Instance.SetNeedsRefresh(true);

            // Implement Scores
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
        public async Task<bool> AddAsync_Score(Score data)
        {
            // Implement
            return false;
        }

        public async Task<bool> UpdateAsync_Score(Score data)
        {
            // Implement
            return false;
        }

        public async Task<bool> DeleteAsync_Score(Score data)
        {
            // Implement
            return false;
        }

            public async Task<Score> GetAsync_Score(string id)
        {
            // Implement
            return null;
        }

        public async Task<IEnumerable<Score>> GetAllAsync_Score(bool forceRefresh = false)
        {
            // Implement
            return null;
        }
        #endregion Score
    }
}