using Crawl.Models;
using System.Diagnostics;

namespace Crawl.GameEngine
{
    class AutoBattleEngine
    {
        public BattleEngine BattleEngine = new BattleEngine(); // Instantiate

        // Automated Battle Sequence.
        public bool ExecuteAutoBattle()
        {
            // Picks 6 Characters
            if (BattleEngine.AddCharactersToBattle() == false)
            {
                // Error, so exit...
                return false;
            }

            // Start
            BattleEngine.StartBattle(true);

            Debug.WriteLine("Battle Start" + " Characters :" + BattleEngine.CharacterList.Count);

            // Initialize the Rounds
            BattleEngine.StartRound();

            // Enum for RoundResult
            RoundEnum RoundResult;

            // Fight Loop. Continue until Game is Over...
            do
            {
                // Do the turn...
                RoundResult = BattleEngine.RoundNextTurn();

                // If the round is over start a new one...
                if (RoundResult == RoundEnum.NewRound)
                {
                    BattleEngine.NewRound();
                    Debug.WriteLine("New Round :" + BattleEngine.BattleScore.RoundCount);
                }

            } while (RoundResult != RoundEnum.GameOver);

            BattleEngine.EndBattle();

            return true;
        }

        // Get current score value.
        public int GetScoreValue()
        {
            return BattleEngine.BattleScore.ScoreTotal;
        }

        // Get current score as object.
        public Score GetScoreObject()
        {
            return BattleEngine.BattleScore;
        }

        // Get number of rounds.
        public int GetRoundCount()
        {
            return BattleEngine.BattleScore.RoundCount;
        }

        // Get Battle results as string.
        public string GetResultsOutput()
        {
            string myResult = null;
                    myResult += "Battle Ended" + BattleEngine.BattleScore.ScoreTotal +
                    " Total Score :" + BattleEngine.BattleScore.ExperienceGainedTotal +
                    " Total Experience :" + BattleEngine.BattleScore.ExperienceGainedTotal +
                    " Rounds :" + BattleEngine.BattleScore.RoundCount +
                    " Turns :" + BattleEngine.BattleScore.TurnCount +
                    " Monster Kills :" + BattleEngine.BattleScore.MonstersKilledList;

            Debug.WriteLine(myResult);

            return myResult;
        }
    }
}
