using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawl.Models;

namespace UnitTests.Models
{
    [TestFixture]
    public class CharacterUnitTests
    {
        [Test]
        public void Character_ScaleLevel_1_Should_Pass()
        {
            // Arrange
            var Test = new Character();
            int Level = 1;
            bool Expected = true;

            // Act
            var Actual = Test.ScaleLevel(Level);

            // Reset

            // Assert
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void Character_ScaleLevel_0_Should_Fail()
        {
            // Arrange
            var Test = new Character();
            int Level = 0;
            bool Expected = false;

            // Act
            var Actual = Test.ScaleLevel(0);

            // Reset

            // Assert
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void Character_ScaleLevel_Neg1_Should_Fail()
        {
            // Arrange
            var Test = new Character();
            int Level = 0;
            bool Expected = false;

            // Act
            var Actual = Test.ScaleLevel(-1);

            // Reset

            // Assert
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void Character_ScaleLevel_Same_Level_Should_Pass()
        {
            // Same level is allowed, because it calculates the values.

            // Arrange
            var Test = new Character();
            int Level = 2;
            bool Expected = true;

            // Set Test level to be 2, Same level should Fail
            Test.Level = Level;

            // Act
            var Actual = Test.ScaleLevel(Level);

            // Reset

            // Assert
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void Character_ScaleLevel_MaxLevel_Plus_Should_Fail()
        {
            // Arrange
            var Test = new Character();
            int Level = LevelTable.MaxLevel+1;
            bool Expected = false;

            // Act
            var Actual = Test.ScaleLevel(Level);

            // Reset

            // Assert
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void Character_ScaleLevel_Lower_Level_Should_Fail()
        {
            // Arrange
            var Test = new Character();
            int Level = 2;
            bool Expected = false;

            // Set Character Level, by Scaling Up...
            Test.ScaleLevel(Level);

            // Act
            var Actual = Test.ScaleLevel(Test.Level - 1);

            // Reset

            // Assert
            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void Character_ScaleLevel_Level_1_Forced_5_Should_Return_MaxHealth_5()
        {
            //Health, Attack, Defense, Speed
            int[] BaseClassBaseStats = { 5, 4, 3, 4 };
            int[] MageClassBaseStats = { 4, 7, 3, 4 };
            int[] KnightClassBaseStats = { 6, 6, 6, 2 };
            int[] AssasinClassBaseStats = { 3, 5, 3, 8 };

            // Base stats.
            int[][] ClassBaseStats = { BaseClassBaseStats, MageClassBaseStats, KnightClassBaseStats, AssasinClassBaseStats };

            // Arrange
            var Test = new Character();
            int Level = 1;
            int Expected = ClassBaseStats[Test.ClassCode()][0];  // Expected MaxHealth

            // Turn on Forced Values
            GameGlobals.SetForcedRandomNumbersValue(5);

            // Act
            var Actual = Test.ScaleLevel(Level);

            // Reset
            GameGlobals.DisableRandomValues();

            // Assert
            Assert.AreEqual(Expected, ClassBaseStats[Test.ClassCode()][0], TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void Character_ScaleLevel_Level_2_Forced_5_Should_Return_MaxHealth_10()
        {
            // Arrange
            var Test = new Character();
            int Level = 2;
            int Expected = 10;  // Expected MaxHealth

            // Turn on Forced Values
            GameGlobals.SetForcedRandomNumbersValue(5);

            // Act
            var Actual = Test.ScaleLevel(Level);

            // Reset
            GameGlobals.DisableRandomValues();

            // Assert
            Assert.AreEqual(Expected, Test.GetHealthMax(), TestContext.CurrentContext.Test.Name);
        }
    }
}
