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

        //Array represents the 20 character levels and relative stats
        //Format: EXP, Attack, Defense, Speed
        int[,] Levels =  {
               {0     ,1,1,1},
               {300   ,1,2,1},
               {900   ,2,3,1},
               {2700  ,2,3,1},
               {6500  ,2,4,2},
               {14000 ,3,4,2},
               {23000 ,3,5,2},
               {34000 ,3,5,2},
               {48000 ,3,5,2},
               {64000 ,4,6,3},
               {85000 ,4,6,3},
               {100000,4,6,3},
               {120000,4,7,3},
               {140000,5,7,3},
               {165000,5,7,4},
               {195000,5,8,4},
               {225000,5,8,4},
               {265000,6,8,4},
               {305000,7,9,4},
               {355000,8,10,5}
          };

        //Returns a character with the default knight class.
        public Character()
        {
            Attribute = new AttributeBase();
            Alive = true;
            Name = "Default"; 
            Class = new KnightClass();

            Level = 1;
            ExperienceTotal = 0;
            RollStats();
            Head = "test";
            Feet = "test";
            Necklass = "test";
            RightFinger = "test";
            LeftFinger = "test";
            Feet = "test";
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
            RollStats();
            Head = "test";
            Feet = "test";
            Necklass = "test";
            PrimaryHand = "None";
            OffHand = "None";
            RightFinger = "test";
            LeftFinger = "test";
            Feet = "test";
        }

        private void RollStats()
        {
            //TODO: ADD more complex math in here to roll the stats based on class starting stats
            Random r = new Random();
            Attribute.MaxHealth = Class.baseHealth + r.Next(-1, 1);
            Attribute.Attack = Class.baseAttack + r.Next(-1, 1);
            Attribute.Defense = Class.baseDefense + r.Next(-1, 1);
            Attribute.Speed = Class.baseSpeed + r.Next(-1, 1);
        }

        public void ReRollStats()
        {
            RollStats();
            ScaleLevel(Level);
        }

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
        public void ScaleLevel(int newLevel)
        {
            Attribute.Attack = Class.baseAttack + Levels[newLevel - 1, 1];
            Attribute.Defense = Class.baseDefense + Levels[newLevel - 1, 2];
            Attribute.Speed = Class.baseAttack + Levels[newLevel - 1, 3];

            //Add code to roll health stats
            for(int i = Level; i <= newLevel; i++)
            {
                
            }
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

        // Returns the Dice for the item Sword 10, is Sword Dice 10
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