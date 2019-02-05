using Crawl.GameEngine;
using Crawl.ViewModels;
using System;
using System.Collections.Generic;

namespace Crawl.Models
{
    // The Character is the higher level concept.  This is the Character with all attirbutes defined.
    public class Character : BaseCharacter
    {
        // Add in the actual attribute class
        public AttributeBase Attribute { get; set; }
        public BaseClass Class { get; set; }

        //Returns a character with the default knight class.
        public Character()
        {
            Attribute = new AttributeBase();
            Alive = true;
            Name = "Default"; 
            Class = new KnightClass();

            Level = 1;
            ExperienceTotal = 0;
        }

        //Main character constructor. "Rolls" stats based on class type.
        public Character(string name, BaseClass classType)
        {
            Attribute = new AttributeBase();
            Alive = true;
            Name = name;
            Class = classType;

            Level = 1;
            ExperienceTotal = 0;

            //TODO: ADD math in here to roll the stats based on class starting stats
            Attribute.MaxHealth = Class.baseHealth;
            Attribute.Attack = Class.baseAttack;
            Attribute.Defense = Class.baseDefense;
            Attribute.Speed = Class.baseSpeed;
        }

        public Character(string name, int Attack, int Defense, int Speed)
        {
            Name = name;
            Attribute = new AttributeBase();
            Alive = true;
            Attribute.Attack = Attack;
            Attribute.Defense = Defense;
            Attribute.Speed = Speed;
            Class = new KnightClass();
        }

        // Make sure Attribute is instantiated in the constructor
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
            Name = newData.Name;
            /*Level = newData.Level;
            ExperienceTotal = newData.ExperienceTotal;
            ImageURI = newData.ImageURI;
            Alive = newData.Alive; 

            // Database information
            Guid = newData.Guid;
            Id = newData.Id;*/

            /*// Populate the Attributes
            AttributeString = newData.AttributeString;

            Attribute = new AttributeBase(newData.AttributeString);

            // Set the strings for the items
            Head = newData.Head;
            Feet = newData.Feet;
            Necklass = newData.Necklass;
            RightFinger = newData.RightFinger;
            LeftFinger = newData.LeftFinger;
            Feet = newData.Feet; */
        }

        // Create a new character, based on existing Character
        public Character(string v, Character newData)
        {
            Name = newData.Name;
        }

        // Upgrades to a set level
        public void ScaleLevel(int level)
        {
            // Implement
        }

        // Update the character information
        // Updates the attribute string
        public void Update(Character newData)
        {
            Attribute.Attack = newData.Attribute.Attack;
            Attribute.Defense = newData.Attribute.Defense;
            Attribute.MaxHealth = newData.Attribute.MaxHealth;
            Attribute.Speed = newData.Attribute.Speed;

            Level = newData.Level;
            ExperienceTotal = newData.Level;

            return;
        }

        // Helper to combine the attributes into a single line, to make it easier to display the item as a string
        public string FormatOutput()
        {
            var myReturn = " Implement";
            return myReturn;
        }

        #region Basics
        // Level Up
        public bool LevelUp()
        {
            // Implement
            return false;
        }

        // Level up to a number, say Level 3
        public int LevelUpToValue(int Value)
        {
            // Implement
            return Level;
        }

        // Add experience
        public bool AddExperience(int newExperience)
        {
            // Implement
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

            // Implement

            // Attack Bonus from Level

            // Get Attack bonus from Items

            return myReturn;
        }

        public int GetSpeed()
        {
            // Base value
            var myReturn = Attribute.Speed;

            // Implement

            // Get Bonus from Level

            // Get bonus from Items

            return myReturn;
        }

        public int GetDefense()
        {
            // Base value
            var myReturn = Attribute.Defense;

            // Implement

            // Get Bonus from Level

            // Get bonus from Items

            return myReturn;
        }

        public int GetHealthMax()
        {
            // Base value
            var myReturn = Attribute.MaxHealth;

            // Implement

            // Get bonus from Items

            return myReturn;
        }

        public int GetHealthCurrent()
        {
            // Base value
            var myReturn = Attribute.CurrentHealth;

            // Implement

            // Get bonus from Items

            return myReturn;
        }

        // Returns the Dice for the itemm Sword 10, is Sword Dice 10
        public int GetDamageDice()
        {
            var myReturn = 0;

            // Implement


            return myReturn;
        }

        // Get the Level based damage
        // Then add the damage for the primary hand item as a Dice Roll
        public int GetDamageRollValue()
        {
            var myReturn = GetLevelBasedDamage();

            // Implement


            return myReturn;
        }

        #endregion GetAttributes

        #region Items
        // Drop All Items
        // Return a list of items for the pool of items
        public List<Item> DropAllItems()
        {
            var myReturn = new List<Item>();

            // Implement

            // Drop all Items

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
            // Implement

            return null;
        }

        // Add Item
        // Looks up the Item
        // Puts the Item ID as a string in the location slot
        // If item is null, then puts null in the slot
        // Returns the item that was in the location
        public Item AddItem(ItemLocationEnum itemlocation, string itemID)
        {
            Item myReturn = new Item();

            // Implement

            return myReturn;
        }

        // Walk all the Items on the 
        
        // Add together all Items that modify the Attribute Enum Passed in
        // Return the sum
        public int GetItemBonus(AttributeEnum attributeEnum)
        {
            var myReturn = 0;
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
            if((Attribute.CurrentHealth - damage) < 0)
            {
                Attribute.CurrentHealth = 0;
                Alive = false;

                //TODO: ADD DROPS HERE
            }
        }
    }
}