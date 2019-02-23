using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Crawl.Models;
using Crawl.ViewModels;
using System.Diagnostics;

namespace Crawl.GameEngine
{
    class RoundEngine : TurnEngine
    {
        // Hold the list of players (monster, and character by guid), and order by speed
        public List<PlayerInfo> PlayerList;

        // Player currently engaged
        public PlayerInfo PlayerCurrent;

        public RoundEngine()
        {
            ClearLists();
        }

        private void ClearLists()
        {
            ItemPool = new List<Item>();
            MonsterList = new List<Monster>();
        }

        // Start the round, need to get the ItemPool, and Characters
        public void StartRound()
        {
            BattleScore.RoundCount = 0;
            NewRound();

            // Debug output for now
            Debug.WriteLine("Start Round :" + BattleScore.RoundCount);
        }

        // Call to start a new Round.
        public void NewRound()
        {          
            EndRound();
            AddMonstersToRound();
            MakePlayerList();

            BattleScore.RoundCount++;
        }

        // Add Monsters
        // Scale them to meet Character Strength...
        private void AddMonstersToRound()
        {
            // Check to see if the monster list is full, if so, no need to add more...
            if (MonsterList.Count() >= 6)
            {
                return;
            }

            var ScaleLevelMax = 2;
            var ScaleLevelMin = 1;

            // Make Sure Monster List exists and is loaded...
            var myMonsterViewModel = MonstersViewModel.Instance;
            myMonsterViewModel.ForceDataRefresh();

            if (myMonsterViewModel.Dataset.Count() > 0)
            {
                // Get 1 monster
                do
                {
                    var rnd = HelperEngine.RollDice(1, myMonsterViewModel.Dataset.Count);
                    {
                        var item = new Monster(myMonsterViewModel.Dataset[rnd - 1]);

                        // Help identify which monster it is...
                        item.Name += " " + (1 + MonsterList.Count()).ToString();

                        var rndScale = HelperEngine.RollDice(ScaleLevelMin, ScaleLevelMax);
                        item.ScaleLevel(rndScale);
                        MonsterList.Add(item);
                    }

                } while (MonsterList.Count() < 6);
            }
            else
            {
                // No monsters in DB, so add 6 new ones...
                for (var i = 0; i < 6; i++)
                {
                    var item = new Monster();
                    // Help identify which monster it is...
                    item.Name += " " + MonsterList.Count() + 1;
                    MonsterList.Add(item);
                }
            }
        }

        // At the end of the round
        // Clear the Item List
        // Clear the Monster List
        public void EndRound()
        {
            // Have each character pickup items...
            foreach (var character in CharacterList)
            {
                PickupItemsFromPool(character);
            }
            ClearLists();
        }

        // Get Round Turn Order

        // Rember Who's Turn

        // RoundNextTurn
        public RoundEnum RoundNextTurn()
        {
            // No characters, game is over...
            if (CharacterList.Count < 1)
            {
                // Game Over
                return RoundEnum.GameOver;
            }

            // Check if round is over
            if (MonsterList.Count < 1)
            {
                // If over, New Round
                return RoundEnum.NewRound;
            }

            // Decide Who gets next turn
            // Remember who just went...
            PlayerCurrent = GetNextPlayerTurn();

            // Decide Who to Attack
            // Do the Turn         
            if (PlayerCurrent.PlayerType == PlayerTypeEnum.Character)
            {
                // Get the player
                var myPlayer = CharacterList.Where(a => a.Guid == PlayerCurrent.Guid).FirstOrDefault();

                // Do the turn....
                TakeTurn(myPlayer);
            }
            // Add Monster turn here...
            else if (PlayerCurrent.PlayerType == PlayerTypeEnum.Monster)
            {
                // Get the player
                var myPlayer = MonsterList.Where(a => a.Guid == PlayerCurrent.Guid).FirstOrDefault();

                // Do the turn....
                TakeTurn(myPlayer);
            }

            return RoundEnum.NextTurn;
        }

        // Recalculate Order and get next turn.
        public PlayerInfo GetNextPlayerTurn()
        {
            OrderPlayerListByTurnOrder();
            var PlayerCurrent = GetNextPlayerInList();
            return PlayerCurrent;
        }

        // Rearrange players by turn order based on remaining characters.
        public void OrderPlayerListByTurnOrder()
        {
            var myReturn = new List<PlayerInfo>();
            MakePlayerList();

            PlayerList = PlayerList.OrderByDescending(a => a.Speed)
                .ThenByDescending(a => a.Level)
                .ThenByDescending(a => a.ExperiencePoints)
                .ThenByDescending(a => a.PlayerType)
                .ThenBy(a => a.Name)
                .ThenBy(a => a.ListOrder)
                .ToList();
        }

        // Make list of players with remaining alive characters.
        private void MakePlayerList()
        {
            PlayerList = new List<PlayerInfo>();
            PlayerInfo tempPlayer;

            var ListOrder = 0;

            foreach (var data in CharacterList)
            {
                if (data.Alive)
                {
                    tempPlayer = new PlayerInfo(data);

                    // Remember the order
                    tempPlayer.ListOrder = ListOrder;
                    PlayerList.Add(tempPlayer);
                    ListOrder++;
                }
            }

            foreach (var data in MonsterList)
            {
                if (data.Alive)
                {
                    tempPlayer = new PlayerInfo(data);

                    // Remember the order
                    tempPlayer.ListOrder = ListOrder;

                    PlayerList.Add(tempPlayer);

                    ListOrder++;
                }
            }
        }

        // Get next player for upcoming turn.
        public PlayerInfo GetNextPlayerInList()
        {
            // Choose last/default player if player not found.
            if (PlayerCurrent == null)
            {
                PlayerCurrent = PlayerList.LastOrDefault();
            }

            // Pick next player in list otherwise.
            for (var i = 0; i < PlayerList.Count(); i++)
            {
                if (PlayerList[i].Guid == PlayerCurrent.Guid)
                {
                    if (i < PlayerList.Count() - 1) // 0 based...
                    {
                        return PlayerList[i + 1];
                    }
                    else
                    {
                        // Return the first in the list...
                        return PlayerList.FirstOrDefault();
                    }
                }
            }
            return null;
        }

        // Have character pickup items from pool after round over.
        public void PickupItemsFromPool(Character character)
        {
            // Upgrade character items

            // If no items in the pool, do nothing.
            if (ItemPool.Count < 1)
            {
                return;
            }

            GetItemFromPoolIfBetter(character, ItemLocationEnum.Head);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.Necklass);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.PrimaryHand);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.OffHand);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.RightFinger);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.LeftFinger);
            GetItemFromPoolIfBetter(character, ItemLocationEnum.Feet);
        }

        // Logic for upgrading items based on stats per character.
        public void GetItemFromPoolIfBetter(Character character, ItemLocationEnum setLocation)
        {
            var myList = ItemPool.Where(a => a.Location == setLocation)
                .OrderByDescending(a => a.Value)
                .ToList();
                
            if (!myList.Any())
            {
                return;
            }

            var currentItem = character.GetItemByLocation(setLocation);
            if (currentItem == null)
            {
                // If no item in the slot then put on the first in the list
                character.AddItem(setLocation, myList.FirstOrDefault().Id);
                return;
            }

            foreach (var item in myList)
            {
                if (item.Value > currentItem.Value)
                {
                    // Put on the new item, which drops the one back to the pool
                    var droppedItem = character.AddItem(setLocation, item.Id);

                    // Remove the item just put on from the pool
                    ItemPool.Remove(item);

                    if (droppedItem != null)
                    {
                        // Add the dropped item to the pool
                        ItemPool.Add(droppedItem);
                    }
                }
            }
        }
    }
}
