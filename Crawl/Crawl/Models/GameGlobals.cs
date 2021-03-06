﻿
// Global switches for the overall game to use...

namespace Crawl.Models
{
    public static class GameGlobals
    {
        // Max number of Players in a Party
        public static int MaxNumberPartyPlayers = 6;

        // Turn on to force Rolls to be non random
        public static bool ForceRollsToNotRandom = false;

        // Holds the random value for the sytem
        private static int _ForcedRandomValue = 1;

        // What number should return for random numbers (1 is good choice...)
        public static int ForcedRandomValue
        {
            get => _ForcedRandomValue;
        }

        // Enable Random Value Override and set the value
        public static int RandomValueOverride = 0;
        public static bool EnableRandomValueOverride = true;

        // What number to use for ToHit values (1,2, 19, 20)
        public static bool ForcingHitValue = false;
        public static int ForcedHitValue = 20;

        public static int ForceToHitValue = 20;

        public static bool ForceOneToMiss = false;
        public static bool ForceTwentyToHit = false;



        // Forces Monsters to hit with a set value
        // Zero, because don't want to add it in unless it is used...
        public static int ForceMonsterDamangeBonusValue = 0;

        // Forces Characters to hit with a set value
        // Zero, because don't want to add it in unless it is used...
        public static int ForceCharacterDamangeBonusValue = 0;

        // Allow Random Items when monsters die...
        public static bool AllowMonsterDropItems = true;

        // Miracle Max...
        public static bool EnableReincarnation = true;

        // Time Warp ...
        // First setting turns it on or off
        // Second setting is the % chance of happening
        // Third setting is a state to show output if currently happening.
        public static bool EnableTimeWarp = true;
        public static int TimeWarpChance = 0;
        public static bool TimeWarpEnabled = false;

        // Turn Off Random Number Generation, and use the passed in values.
        public static void SetForcedRandomNumbersValueAndToHit(int value, int hit)
        {
            SetForcedRandomNumbersValue(value);
            ForceToHitValue = hit;
        }

        // Turn Off Random Number Generation, and use the passed in values.
        public static void SetForcedRandomNumbersValue(int value)
        {
            EnableRandomValues();
            _ForcedRandomValue = value;
        }

        // Flip the Random State (false to true etc...)
        // Call this after setting, to restore...
        public static void ToggleRandomState()
        {
            ForceRollsToNotRandom = !ForceRollsToNotRandom;
        }

        // Turn Random State Off
        public static void DisableRandomValues()
        {
            ForceRollsToNotRandom = false;
        }

        // Turn Random State On
        public static void EnableRandomValues()
        {
            ForceRollsToNotRandom = true;
        }

        // Debug Settings
        public static bool EnableCriticalMissProblems = true;
        public static bool EnableCriticalHitDamage = true;
    }
}
