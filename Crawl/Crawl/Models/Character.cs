﻿using Crawl.GameEngine;
using Crawl.ViewModels;
using System;
using System.Collections.Generic;
using SQLite;
using System.Diagnostics;

namespace Crawl.Models
{
    // The Character is the higher level concept.  This is the Character with all attirbutes defined.
    public class Character : BaseCharacter
    {
        // Add in the actual attribute class
        public AttributeBase Attribute { get; set; }


        //Health, Attack, Defense, Speed
        static int[] BaseClassBaseStats = { 5, 4, 3, 4 };
        static int[] MageClassBaseStats = { 4, 7, 3, 4 };
        static int[] KnightClassBaseStats = { 6, 6, 6, 2 };
        static int[] AssasinClassBaseStats = { 3, 5, 3, 8 };

        // Base stats.
        static int[][] ClassBaseStats = { BaseClassBaseStats, MageClassBaseStats, KnightClassBaseStats, AssasinClassBaseStats };

        // Buffs
        private int HealthBuff = 0;
        private int SpeedBuff = 0;
        private int DefenseBuff = 0;
        private int AttackBuff = 0;

        //Returns a character with the default knight class.
        public Character()
        {
            Attribute = new AttributeBase();
            Alive = true;
            Name = "Default";
            Type = ClassTypeEnum.Unknown;

            Level = 1;
            ExperienceTotal = 0;
            RollStats();
            Head = Feet = Necklass = PrimaryHand = OffHand = RightFinger = LeftFinger = "None";
        }

        // Death
        // Alive turns to False
        public override void CauseDeath()
        {
            if (GameGlobals.EnableReincarnation)
            {
                // If reincarnation is turned on, use it here.

                if (ReincarnationNumberOfLives > 0)
                {
                    ReincarnationNumberOfLives--;
                    Attribute.CurrentHealth = Attribute.MaxHealth;
                    Debug.Write("Reincarnation Happened, thanks Mircale Max");

                    Alive = true; // Death did not happen
                    return;
                }
            }

            Alive = false;
        }


        //Main character constructor. "Rolls" stats based on class type.
        public Character(string name, ClassTypeEnum classType)
        {
            Attribute = new AttributeBase();
            Alive = true;
            Level = 1;
            ExperienceTotal = 0;
            Name = name;
            Type = classType;
            RollStats();

            Head = Feet = Necklass = PrimaryHand = OffHand = RightFinger = LeftFinger = "None";
        }

        // Roll Stats for Character.
        public void RollStats()
        {
            //Roll character buffs
            int HealthBuff = 0;// Dice.Roll(4, 1);
            int SpeedBuff = 0;// Dice.Roll(4, 1);
            int DefenseBuff = 0;// Dice.Roll(4, 1);
            int AttackBuff = 0;// Dice.Roll(4, 1);

            //Set attributes 
            ScaleLevel(Level);
        }

        // Constructor with string.
        public Character(string v)
        {
            Attribute = new AttributeBase();
            Alive = true;
        }

        // Create a new character, based on a passed in BaseCharacter
        // Used for converting from database format to character
        public Character(BaseCharacter newData)
        {
            // Base information
            Alive = newData.Alive;
            Name = newData.Name;
            Description = newData.Description;
            Level = newData.Level;
            ExperienceTotal = newData.ExperienceTotal;
            ImageURI = newData.ImageURI;
            Type = newData.Type;

            // Database information
            Guid = newData.Guid;
            Id = newData.Id;

            // Populate the Attributes
            AttributeString = newData.AttributeString;
            Attribute = new AttributeBase(newData.AttributeString);

            // Set the strings for the items
            Head = newData.Head;
            Feet = newData.Feet;
            Necklass = newData.Necklass;
            RightFinger = newData.RightFinger;
            LeftFinger = newData.LeftFinger;
            Feet = newData.Feet;
            RollStats();
        }

        // Create a new character, based on existing Character
        public Character(string v, Character newData)
        {
            Name = newData.Name;
        }

        // Upgrades to a set level
        public bool ScaleLevel(int newLevel)
        {
            LevelTable lt = new LevelTable();

            // Return false if the requested level is too high
            if (newLevel > LevelTable.MaxLevel)
                return false;

            // Return false if the requested level is too low
            if (newLevel < this.Level)
                return false;

            Attribute.Attack = baseAttack() + AttackBuff + lt.LevelDetailsList[newLevel].Attack;
            Attribute.Defense = baseDefense() + DefenseBuff + lt.LevelDetailsList[newLevel].Defense;
            Attribute.Speed = baseSpeed() + SpeedBuff + lt.LevelDetailsList[newLevel].Speed;

            Attribute.MaxHealth = baseHealth() + HealthBuff;
            Attribute.MaxHealth += HelperEngine.RollDice(newLevel - Level, 10);

            return true;
        }

        // Update the character information
        // Updates the attribute string
        public void Update(Character newData)
        {
            if (newData == null)
                return;

            // Update all the fields in the Data, except for the Id

            // Base information
            Name = newData.Name;
            Description = newData.Description;
            Level = newData.Level;
            ExperienceTotal = newData.ExperienceTotal;
            ImageURI = newData.ImageURI;
            Alive = newData.Alive;
            Type = newData.Type;

            // Database information
            Guid = newData.Guid;
            Id = newData.Id;

            // Populate the Attributes
            Attribute = newData.Attribute;

            // set the attribute string, for the Attribute
            AttributeString = AttributeBase.GetAttributeString(Attribute);

            // Set the strings for the items
            Head = newData.Head;
            Feet = newData.Feet;
            Necklass = newData.Necklass;
            RightFinger = newData.RightFinger;
            LeftFinger = newData.LeftFinger;
            Feet = newData.Feet;
            RollStats();
        }

        // Helper to combine the attributes into a single line, to make it easier to display the item as a string
        public string FormatOutput()
        {
            var myReturn = string.Empty;
            myReturn += Name;
            myReturn += " , " + Description;
            myReturn += " , Level : " + Level.ToString();
            myReturn += " , Total Experience : " + ExperienceTotal;
            myReturn += " , " + Attribute.FormatOutput();
            myReturn += " , Items : " + ItemSlotsFormatOutput();
            myReturn += " Damage : " + GetDamageDice();

            return myReturn;
        }

        #region Basics
        // Level Up
        public bool LevelUp()
        {
            // Walk the Level Table descending order
            // Stop when experience is >= experience in the table
            for (var i = LevelTable.Instance.LevelDetailsList.Count - 1; i > 0; i--)
            {
                // Check the Level
                // If the Level is > Experience for the Index, increment the Level.
                if (LevelTable.Instance.LevelDetailsList[i].Experience <= ExperienceTotal)
                {
                    var NewLevel = LevelTable.Instance.LevelDetailsList[i].Level;

                    // When leveling up, the current health is adjusted up by an offset of the MaxHealth, rather than full restore
                    var OldCurrentHealth = Attribute.CurrentHealth;
                    var OldMaxHealth = Attribute.MaxHealth;

                    // Set new Health
                    // New health, is d10 of the new level.  So leveling up 1 level is 1 d10, leveling up 2 levels is 2 d10.
                    var NewHealthAddition = HelperEngine.RollDice(NewLevel - Level, 10);

                    // Increment the Max health
                    Attribute.MaxHealth += NewHealthAddition;

                    // Calculate new current health
                    // old max was 10, current health 8, new max is 15 so (15-(10-8)) = current health
                    Attribute.CurrentHealth = (Attribute.MaxHealth - (OldMaxHealth - OldCurrentHealth));

                    // Refresh the Attriburte String
                    AttributeString = AttributeBase.GetAttributeString(this.Attribute);

                    // Set the new level
                    Level = NewLevel;

                    // Done, exit
                    return true;
                }
            }

            return false;
        }

        // Level up to a number, say Level 3
        public int LevelUpToValue(int Value)
        {
            // Adjust the experience to the min for that level.
            // That will trigger level up to happen

            if (Value < 0)
            {
                // Skip, and return old level
                return Level;
            }

            if (Value <= Level)
            {
                // Skip, and return old level
                return Level;
            }

            if (Value > LevelTable.MaxLevel)
            {
                Value = LevelTable.MaxLevel;
            }

            AddExperience(LevelTable.Instance.LevelDetailsList[Value].Experience + 1);

            return Level;
        }

        // Add experience
        public bool AddExperience(int newExperience)
        {
            // Don't allow going lower in experience
            if (newExperience < 0)
            {
                return false;
            }

            // Increment the Experience
            ExperienceTotal += newExperience;

            // Can't level UP if at max.
            if (Level == LevelTable.MaxLevel)
            {
                return false;
            }

            // Then check for Level UP
            // If experience is higher than the experience at the next level, level up is OK.
            if (ExperienceTotal >= LevelTable.Instance.LevelDetailsList[Level + 1].Experience)
            {
                return LevelUp();
            }
            return false;
        }

        #endregion Basics

        #region GetAttributes
        // Get Attributes

        // Get Attack
        public int GetAttack()
        {
            // Base Attack
            var myReturn = Attribute.Attack;

            // Attack Bonus from Level
            myReturn += LevelTable.Instance.LevelDetailsList[Level].Attack;

            // Get Attack bonus from Items
            myReturn += GetItemBonus(AttributeEnum.Attack);

            return myReturn;
        }

        // Get Speed
        public int GetSpeed()
        {
            // Base value
            var myReturn = Attribute.Speed;

            // Get Bonus from Level
            myReturn += LevelTable.Instance.LevelDetailsList[Level].Speed;

            // Get bonus from Items
            myReturn += GetItemBonus(AttributeEnum.Speed);

            return myReturn;
        }

        // Get Defense
        public int GetDefense()
        {
            // Base value
            var myReturn = Attribute.Defense;

            // Get Bonus from Level
            myReturn += LevelTable.Instance.LevelDetailsList[Level].Defense;

            // Get bonus from Items
            myReturn += GetItemBonus(AttributeEnum.Defense);

            return myReturn;
        }

        // Get Max Health
        public int GetHealthMax()
        {
            // Base value
            var myReturn = Attribute.MaxHealth;

            // Get bonus from Items
            myReturn += GetItemBonus(AttributeEnum.MaxHealth);

            return myReturn;
        }

        // Get Current Health
        public int GetHealthCurrent()
        {
            // Base value
            var myReturn = Attribute.CurrentHealth;

            // Get bonus from Items
            myReturn += GetItemBonus(AttributeEnum.CurrentHealth);

            return myReturn;
        }

        // Returns the Dice for the item
        // Sword 10, is Sword Dice 10
        public int GetDamageDice()
        {
            var myReturn = 0;

            var myItem = ItemsViewModel.Instance.GetItem(PrimaryHand);
            if (myItem != null)
            {
                // Damage is base damage plus dice of the weapon.  So sword of Damage 10 is d10
                myReturn += myItem.Damage;
            }

            return myReturn;
        }

        // Get the Level based damage
        // Then add the damage for the primary hand item as a Dice Roll
        public int GetDamageRollValue()
        {
            var myReturn = GetLevelBasedDamage();

            var myItem = ItemsViewModel.Instance.GetItem(PrimaryHand);
            if (myItem != null)
            {
                // Damage is base damage plus dice of the weapon.  So sword of Damage 10 is d10
                myReturn += HelperEngine.RollDice(1, myItem.Damage);
            }

            return myReturn;
        }

        #endregion GetAttributes

        #region Items
        // Drop All Items
        // Return a list of items for the pool of items
        public List<Item> DropAllItems()
        {
            var myReturn = new List<Item>();

            // Drop all Items
            Item myItem;

            myItem = RemoveItem(ItemLocationEnum.Head);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }

            myItem = RemoveItem(ItemLocationEnum.Necklass);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }

            myItem = RemoveItem(ItemLocationEnum.PrimaryHand);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }

            myItem = RemoveItem(ItemLocationEnum.OffHand);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }

            myItem = RemoveItem(ItemLocationEnum.RightFinger);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }

            myItem = RemoveItem(ItemLocationEnum.LeftFinger);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }

            myItem = RemoveItem(ItemLocationEnum.Feet);
            if (myItem != null)
            {
                myReturn.Add(myItem);
            }

            return myReturn;
        }

        // Remove Item from a set location
        // Does this by adding a new item of Null to the location
        // This will return the previous item, and put null in its place
        // Returns the item that was at the location
        // Nulls out the location
        public Item RemoveItem(ItemLocationEnum itemlocation)
        {
            var myReturn = AddItem(itemlocation, null);

            // Save Changes
            return myReturn;
        }

        // Get the Item at a known string location (head, foot etc.)
        public Item GetItem(string itemString)
        {
            return ItemsViewModel.Instance.GetItem(itemString);
        }

        // Get the Item at a known string location (head, foot etc.)
        public Item GetItemByLocation(ItemLocationEnum itemLocation)
        {
            switch (itemLocation)
            {
                case ItemLocationEnum.Head:
                    return GetItem(Head);

                case ItemLocationEnum.Necklass:
                    return GetItem(Necklass);

                case ItemLocationEnum.PrimaryHand:
                    return GetItem(PrimaryHand);

                case ItemLocationEnum.OffHand:
                    return GetItem(OffHand);

                case ItemLocationEnum.RightFinger:
                    return GetItem(RightFinger);

                case ItemLocationEnum.LeftFinger:
                    return GetItem(LeftFinger);

                case ItemLocationEnum.Feet:
                    return GetItem(Feet);
            }

            return null;
        }

        // Add Item
        // Looks up the Item
        // Puts the Item ID as a string in the location slot
        // If item is null, then puts null in the slot
        // Returns the item that was in the location
        public Item AddItem(ItemLocationEnum itemlocation, string itemID)
        {
            Item myReturn;

            switch (itemlocation)
            {
                case ItemLocationEnum.Feet:
                    myReturn = GetItem(Feet);
                    Feet = itemID;
                    break;

                case ItemLocationEnum.Head:
                    myReturn = GetItem(Head);
                    Head = itemID;
                    break;

                case ItemLocationEnum.Necklass:
                    myReturn = GetItem(Necklass);
                    Necklass = itemID;
                    break;

                case ItemLocationEnum.PrimaryHand:
                    myReturn = GetItem(PrimaryHand);
                    PrimaryHand = itemID;
                    break;

                case ItemLocationEnum.OffHand:
                    myReturn = GetItem(OffHand);
                    OffHand = itemID;
                    break;

                case ItemLocationEnum.RightFinger:
                    myReturn = GetItem(RightFinger);
                    RightFinger = itemID;
                    break;

                case ItemLocationEnum.LeftFinger:
                    myReturn = GetItem(LeftFinger);
                    LeftFinger = itemID;
                    break;

                default:
                    myReturn = null;
                    break;
            }

            return myReturn;
        }

        // Walk all the Items on the Character.
        // Add together all Items that modify the Attribute Enum Passed in
        // Return the sum
        public int GetItemBonus(AttributeEnum attributeEnum)
        {
            var myReturn = 0;
            Item myItem;

            myItem = ItemsViewModel.Instance.GetItem(Head);
            if (myItem != null)
            {
                if (myItem.Attribute == attributeEnum)
                {
                    myReturn += myItem.Value;
                }
            }

            myItem = ItemsViewModel.Instance.GetItem(Necklass);
            if (myItem != null)
            {
                if (myItem.Attribute == attributeEnum)
                {
                    myReturn += myItem.Value;
                }
            }

            myItem = ItemsViewModel.Instance.GetItem(PrimaryHand);
            if (myItem != null)
            {
                if (myItem.Attribute == attributeEnum)
                {
                    myReturn += myItem.Value;
                }
            }

            myItem = ItemsViewModel.Instance.GetItem(OffHand);
            if (myItem != null)
            {
                if (myItem.Attribute == attributeEnum)
                {
                    myReturn += myItem.Value;
                }
            }

            myItem = ItemsViewModel.Instance.GetItem(RightFinger);
            if (myItem != null)
            {
                if (myItem.Attribute == attributeEnum)
                {
                    myReturn += myItem.Value;
                }
            }

            myItem = ItemsViewModel.Instance.GetItem(LeftFinger);
            if (myItem != null)
            {
                if (myItem.Attribute == attributeEnum)
                {
                    myReturn += myItem.Value;
                }
            }

            myItem = ItemsViewModel.Instance.GetItem(Feet);
            if (myItem != null)
            {
                if (myItem.Attribute == attributeEnum)
                {
                    myReturn += myItem.Value;
                }
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
            if (damage < 1)
            {
                return;
            }

            Attribute.CurrentHealth -= damage;
            if (GetHealthCurrent() <= 0)
            {
                // Death...
                CauseDeath();
            }
        }

        // Get Class ID.
        public String ClassName
        {
            get { return Type.ToString(); }
        }

        // Choose Class.
        public int ClassCode()
        {
            switch (Type)
            {
                default: return 0;
                case ClassTypeEnum.Mage: return 1;
                case ClassTypeEnum.Knight: return 2;
                case ClassTypeEnum.Assassin: return 3;
            }
        }

        //Functions to get base attributes for the class of this character
        public int baseHealth()
        {
            return ClassBaseStats[ClassCode()][0];
        }
        public int baseAttack()
        {
            return ClassBaseStats[ClassCode()][1];
        }
        public int baseDefense() { return ClassBaseStats[ClassCode()][2]; }
        public int baseSpeed() { return ClassBaseStats[ClassCode()][3]; }
    }
}
