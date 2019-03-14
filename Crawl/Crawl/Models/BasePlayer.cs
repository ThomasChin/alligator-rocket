using System;

namespace Crawl.Models
{
    // Base Player is either a Monster or a Character.  It has the shared aspects of both...

    public class BasePlayer<T> : BasePlayerItemSlots<T>
    {

        // Level of the character, or difficulty level of the monster
        public int Level { get; set; }
         
        // Current experience gained, or to give
        public int ExperienceTotal { get; set; }

        // Dice for leveling up. Default is d10.
        public int HealthDice { get; set; } = 10;

        public bool Alive { get; set; }

        // If Reincarnation is Enabled, how many times can it happen on this character?
        public int ReincarnationNumberOfLives = 1;

        // The AttributeString will be unpacked and stored in the top level of Character as actual attributes, 
        // but it needs to go here as a string so it can be saved to the database.
        public string AttributeString { get; set; }


        // Death
        // Alive turns to False
        public virtual void CauseDeath()
        {
           // var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
           // player.Load("Dun-dun-duuuuun.mp3");
           // player.Play();

            Alive = false;
        }

        // Get Level based Damage
        // 1/4 of the Level of the Player is the base damage they do.
        public int GetLevelBasedDamage()
        {
            return (int)Math.Ceiling(Level * .25);
        }
    }
}
