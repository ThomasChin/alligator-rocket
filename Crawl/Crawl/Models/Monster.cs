using System;
using SQLite;
using Crawl.Controllers;
using Crawl.ViewModels;
using System.Collections.Generic;
using Crawl.GameEngine;

public enum MonsterType {Base = 0, GiantSquid, GiantStarfish, GiantWhale}

namespace Crawl.Models
{
    // The Monster is the higher level concept.  This is the Character with all attirbutes defined.
    public class Monster : BaseMonster
    {
        [PrimaryKey]
        public string Id { get; set; }

        //Type is effectively the monster's class
        public MonsterType type { get; set; }
        public int Difficulty { get; set; }

        public const int MINDIFF = 1;
        public const int MAXDIFF = 5;

        // Remaining Experience Points to give
        public int ExperienceRemaining { get; set; }

        // Add in the actual attribute class
        public AttributeBase Attribute { get; set; }

        // Make sure Attribute is instantiated in the constructor
        public Monster()
        {
            //Default name is Monster, 
            Name = "Monster";
            Attribute = new AttributeBase();
            Alive = true;
            Level = 1;
            type = MonsterType.GiantSquid;
            Difficulty = 1;

        }

        public Monster(string name, MonsterType newtype, int maxhealth, int attack, int defense, int speed, int level, int difficulty)
        {
            type = newtype;
            Name = name;
            Attribute = new AttributeBase();
            Attribute.MaxHealth = maxhealth;
            Attribute.Attack = attack;
            Attribute.Defense = defense;
            Attribute.Speed = speed;

            Alive = true;
            Level = level;
            ScaleLevel(level);
            Difficulty = difficulty;
        }

        // Passed in from creating via the Database, so use the guid passed in...
        public Monster(BaseMonster newData)
        {
            // Implement
        }

        // Returns the string name of the monster type
        public string GetMonsterType()
        {
            return type.ToString();
        }

        //Picks new and randomized stats for the monster
        public void RollStats()
        {
            Attribute.MaxHealth = 10 + HelperEngine.RollDice(3, 1);
            Attribute.Attack = 10 + HelperEngine.RollDice(3, 1);
            Attribute.Defense = 10 + HelperEngine.RollDice(3, 1);
            Attribute.Speed = 10 + HelperEngine.RollDice(3, 1);
        }

        // Upgrades a monster to a set level
        public void ScaleLevel(int level)
        {
            //TODO: ADD base attributes
            Level = level;

            LevelTable lt = new LevelTable();

            Attribute.Attack =  lt.LevelDetailsList[level].Attack;
            Attribute.Defense = lt.LevelDetailsList[level].Defense;
            Attribute.Speed = lt.LevelDetailsList[level].Speed;

            Attribute.MaxHealth = Dice.Roll(10, level);
        }

        // Update the values passed in
        public new void Update(Monster newData)
        {
            type = newData.type;
            Name = newData.Name;
            Level = newData.Level;
            ScaleLevel(newData.Level);
        }

        // Helper to combine the attributes into a single line, to make it easier to display the item as a string
        public string FormatOutput()
        {
            var UniqueOutput = "Implement";
            var myReturn = "Implement";

            myReturn += " , Unique Item : " + UniqueOutput;
            return myReturn;
        }

        // Calculate How much experience to return
        // Formula is the % of Damage done up to 100%  times the current experience
        // Needs to be called before applying damage
        public int CalculateExperienceEarned(int damage)
        {
            int damageTaken = (Attribute.MaxHealth - Attribute.CurrentHealth)/Attribute.MaxHealth;
            return damageTaken * ExperienceTotal;

        }

        #region GetAttributes
        // Get Attributes

        // Get Attack
        public int GetAttack()
        {
            // Base Attack
            var myReturn = Attribute.Attack;

            return myReturn;
        }

        // Get Speed
        public int GetSpeed()
        {
            // Base value
            var myReturn = Attribute.Speed;

            return myReturn;
        }

        // Get Defense
        public int GetDefense()
        {
            // Base value
            var myReturn = Attribute.Defense;

            return myReturn;
        }

        // Get Max Health
        public int GetHealthMax()
        {
            // Base value
            var myReturn = Attribute.MaxHealth;

            return myReturn;
        }

        // Get Current Health
        public int GetHealthCurrent()
        {
            // Base value
            var myReturn = Attribute.CurrentHealth;

            return myReturn;
        }

        // Get the Level based damage
        // Then add in the monster damage
        public int GetDamage()
        {
            var myReturn = 0;
            myReturn += Damage;

            return myReturn;
        }

        // Get the Level based damage
        // Then add the damage for the primary hand item as a Dice Roll
        public int GetDamageRollValue()
        {
            return GetDamage();
        }

        #endregion GetAttributes

        #region Items
        // Gets the unique item (if any) from this monster when it dies...
        public Item GetUniqueItem()
        {
            var myReturn = ItemsViewModel.Instance.GetItem(UniqueItem);

            return myReturn;
        }

        // Drop all the items the monster has
        public List<Item> DropAllItems()
        {
            var myReturn = new List<Item>();

            // Drop all Items
            Item myItem;

            // Implement

            return myReturn;
        }

        #endregion Items

        // Take Damage
        // If the damage recived, is > health, then death occurs
        // Return the number of experience received for this attack 
        // monsters give experience to characters.  Characters don't accept expereince from monsters
        public void TakeDamage(int damage)
        {
            if (Attribute.CurrentHealth - damage <= 0)
                Alive = false;

            else
                Attribute.CurrentHealth -= damage;
        }
    }
}