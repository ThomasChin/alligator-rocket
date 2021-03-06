﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Crawl.Models;
using Crawl.ViewModels;

namespace Crawl.GameEngine
{
    // A battle has
    public class BattleEngine : RoundEngine
    {
        // The status of the actual battle, running or not (over)
        private bool isBattleRunning = false;

        // Constructor calls Init
        public BattleEngine()
        {
            BattleEngineClearData();
        }

        // Sets the new state for the variables for Battle
        public void BattleEngineClearData()
        {
            BattleScore = new Score();
            BattleMessages = new BattleMessages();
            RoundStateEnum = RoundEnum.NewRound;

            ItemPool.Clear();
            MonsterList.Clear();
            CharacterList.Clear();

            // Reset current player
            PlayerCurrent = null;
        }

        // Determine if Auto Battle is On or Off
        public bool GetAutoBattleState()
        {
            return BattleScore.AutoBattle;
        }

        // Return if the Battle is Still running
        public bool BattleRunningState()
        {
            return isBattleRunning;
        }

        // Initializes the Battle to begin
        public bool StartBattle(bool isAutoBattle)
        {
            BattleScore.AutoBattle = isAutoBattle;
            isBattleRunning = true;

            // Check for at least 1 party member.
            if (CharacterList.Count < 1) { return false; }
            return true;
        }

        // End battle and Update Battle State, Log Score to Database
        public void EndBattle()
        {
            // Set Score.
            BattleScore.ScoreTotal = BattleScore.ExperienceGainedTotal;

            // Turn off battle.
            isBattleRunning = false;

            // Save score.
            // Score needs to be saved outside the battle engine...
            // ScoresViewModel.Instance.AddAsync(BattleScore).GetAwaiter().GetResult();
        }

        // Add Characters
        // Scale them to meet Character Strength...
        public bool AddCharactersToBattle()
        {
            // Check to see if the Character list is full, if so, no need to add more...
            if (CharacterList.Count >= 1)
            {
                return true;
            }

            var ScaleLevelMax = 2;
            var ScaleLevelMin = 1;

            if (CharactersViewModel.Instance.Dataset.Count < 1)
            {
                return false;
            }

            // Get 6 Characters
            do
            {
                var myData = GetRandomCharacter(ScaleLevelMin, ScaleLevelMax);
                CharacterList.Add(myData);
            } while (CharacterList.Count < 1);

            return true;
        }

        // Get a random character.
        public Character GetRandomCharacter(int ScaleLevelMin, int ScaleLevelMax)
        {
            var myCharacterViewModel = CharactersViewModel.Instance;

            var rnd = HelperEngine.RollDice(1, myCharacterViewModel.Dataset.Count);

            var myData = new Character(myCharacterViewModel.Dataset[rnd - 1]);

            // Help identify which Character it is...
            myData.Name += " " + (1 + CharacterList.Count).ToString();

            var rndScale = HelperEngine.RollDice(ScaleLevelMin, ScaleLevelMax);
            myData.ScaleLevel(rndScale);

            // Add Items to Character
            myData.Head = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.Head, AttributeEnum.Unknown);
            myData.Necklass = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.Necklass, AttributeEnum.Unknown);
            myData.PrimaryHand = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.PrimaryHand, AttributeEnum.Unknown);
            myData.OffHand = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.OffHand, AttributeEnum.Unknown);
            myData.RightFinger = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.RightFinger, AttributeEnum.Unknown);
            myData.LeftFinger = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.LeftFinger, AttributeEnum.Unknown);
            myData.Feet = ItemsViewModel.Instance.ChooseRandomItemString(ItemLocationEnum.Feet, AttributeEnum.Unknown);

            return myData;
        }

        /// <summary>
        /// Retruns a formated String of the Results of the Battle
        /// </summary>
        /// <returns></returns>
        public string GetResultsOutput()
        {

            string myResult = "" +
                    " Battle Ended" + BattleScore.ScoreTotal +
                    " Total Score :" + BattleScore.ExperienceGainedTotal +
                    " Total Experience :" + BattleScore.ExperienceGainedTotal +
                    " Rounds :" + BattleScore.RoundCount +
                    " Turns :" + BattleScore.TurnCount +
                    " Monster Kills :" + BattleScore.MonstersKilledList;

            Debug.WriteLine(myResult);

            return myResult;
        }

        public void EmptyItemPool()
        {
            ItemPool = new List<Item>();
        }
    }
}
