using System;
using SQLite;
using Crawl.Controllers;
using Crawl.ViewModels;
using System.Collections.Generic;
using Crawl.GameEngine;
using System.Diagnostics;

namespace Crawl.Models
{
    // The Monster is the higher level concept.  This is the Character with all attirbutes defined.
    public class Monster : BaseMonster
    {

        // Enum for MonsterType
        public MonsterTypeEnum Type { get; set; }

        // Difficulty
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
            Type = MonsterTypeEnum.GiantSquid;
            Difficulty = 1;

        }

        //Basic constructor with inputs for base stats
        public Monster(string name, MonsterTypeEnum type, int maxhealth, int attack, int defense, int speed, int level, int difficulty)
        {
            Type = type;
            Name = name;
            Attribute = new AttributeBase();
            Attribute.MaxHealth = maxhealth;
            Attribute.Attack = attack;
            Attribute.Defense = defense;
            Attribute.Speed = speed;
            Attribute.CurrentHealth = Attribute.MaxHealth;

            Alive = true;
            Level = level;
            ScaleLevel(level);
            Difficulty = difficulty;
        }

        // Passed in from creating via the Database, so use the guid passed in...
        public Monster(BaseMonster newData)
        {
            Name = newData.Name;
            Attribute = new AttributeBase();
            Type = newData.Type;

            Alive = true;
            Level = newData.Level;

            Id = System.Guid.NewGuid().ToString();
        }

        // Returns the string name of the monster type
        public string GetMonsterType()
        {
            return Type.ToString();
        }

        //Picks new and randomized stats for the monster
        public void RollStats()
        {
            Attribute.MaxHealth = 1000 + HelperEngine.RollDice(3, 10);
            Attribute.Attack = 1000 + HelperEngine.RollDice(3, 10);
            Attribute.Defense = 1000 + HelperEngine.RollDice(3, 10);
            Attribute.Speed = 1000 + HelperEngine.RollDice(3, 10);
            Attribute.CurrentHealth = Attribute.MaxHealth;
        }

        // Upgrades a monster to a set level
        public void ScaleLevel(int level)
        {
            Level = level;

            // Get the number of points at the next level, and set it for Experience Total...
            ExperienceTotal = LevelTable.Instance.LevelDetailsList[Level + 1].Experience;
            ExperienceRemaining = ExperienceTotal;

            LevelTable lt = new LevelTable();

            Attribute.Attack = lt.LevelDetailsList[level].Attack;
            Attribute.Defense = lt.LevelDetailsList[level].Defense;
            Attribute.Speed = lt.LevelDetailsList[level].Speed;

            Attribute.MaxHealth = HelperEngine.RollDice(level - Level, 10);
            Attribute.CurrentHealth = Attribute.MaxHealth;
        }

        // Update the values passed in
        public new void Update(Monster newData)
        {
            if (newData == null)
                return; 

            Type = newData.Type;
            Name = newData.Name;
            Level = newData.Level;
            ScaleLevel(newData.Level);

            // Database information
            Guid = newData.Guid;
            Id = newData.Id;
        }

        // Helper to combine the attributes into a single line, to make it easier to display the item as a string
        public string FormatOutput()
        {
            var UniqueOutput = "None";
            var myUnique = ItemsViewModel.Instance.GetItem(UniqueItem);
            if (myUnique != null)
            {
                UniqueOutput = myUnique.FormatOutput();
            }

            var myReturn = Name;
            myReturn += " , " + Description;
            myReturn += " , Level : " + Level.ToString();
            myReturn += " , Total Experience : " + ExperienceTotal;
            myReturn += " , Unique Item : " + UniqueOutput;

            return myReturn;
        }

        // Calculate How much experience to return
        // Formula is the % of Damage done up to 100%  times the current experience
        // Needs to be called before applying damage
        public int CalculateExperienceEarned(int damage)
        {
            if (damage < 1)
            {
                return 0;
            }

            int remainingHealth = Math.Max(Attribute.CurrentHealth - damage, 0); // Go to 0 is OK...
            double rawPercent = (double)remainingHealth / (double)Attribute.CurrentHealth;
            double deltaPercent = 1 - rawPercent;
            var pointsAllocate = (int)Math.Floor(ExperienceRemaining * deltaPercent);

            // Catch rounding of low values, and force to 1.
            if (pointsAllocate < 1)
            {
                pointsAllocate = 1;
            }

            // Take away the points from remaining experience
            ExperienceRemaining -= pointsAllocate;
            if (ExperienceRemaining < 0)
            {
                pointsAllocate = 0;
            }

            return pointsAllocate;
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

            myItem = ItemsViewModel.Instance.GetItem(UniqueItem);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }
            return myReturn;
        }

        #endregion Items

        // Take Damage
        // If the damage recived, is > health, then death occurs
        // Return the number of experience received for this attack 
        // monsters give experience to characters.  Characters don't accept expereince from monsters
        public void TakeDamage(int damage)
        {
            Debug.WriteLine("DAMAGE:" + (Attribute.CurrentHealth - damage));
            if (Attribute.CurrentHealth - damage <= 0)
            {
                Alive = false;
                Attribute.CurrentHealth = 0;
            }

            else
                Attribute.CurrentHealth -= damage;
        }
    }
}
