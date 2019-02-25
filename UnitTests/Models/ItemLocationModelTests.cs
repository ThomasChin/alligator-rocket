using NUnit.Framework;
using Crawl.Models;
using System.Linq;
using System;

namespace UnitTests.Models
{
    [TestFixture]
    public class ItemLocationTests
    {
        [Test]
        public void Model_ItemLocationList_GetListCharacter_Should_Pass()
        {
            // Instantiate a new ItemLocation Base, should have default of 1 for all values
            var myDataList = ItemLocationList.GetListCharacter;

            // Get Expected set
            var myList = Enum.GetNames(typeof(ItemLocationEnum)).ToList();
            var myExpectedList = myList.Where(a =>
                                          a.ToString() != ItemLocationEnum.Unknown.ToString() &&
                                           a.ToString() != ItemLocationEnum.Finger.ToString()
                                            )
                                            .OrderBy(a => a)
                                            .ToList();

            // Make sure each item is in the list
            foreach (var item in myDataList)
            {
                var found = false;
                foreach (var expected in myExpectedList)
                {
                    if (item == expected)
                    {
                        found = true;
                        break;
                    }
                }
                Assert.AreEqual(true, found, "item : " + item + TestContext.CurrentContext.Test.Name);
            }

            // reverse it, to make sure the list has each item
            // Make sure each item is in the list
            foreach (var expected in myExpectedList)
            {
                var found = false;
                {
                    foreach (var item in myDataList)
                        if (item == expected)
                        {
                            found = true;
                            break;
                        }
                }
                Assert.AreEqual(true, found, "expected : " + expected + TestContext.CurrentContext.Test.Name);
            }

        }

        [Test]
        public void Model_ItemLocationList_GetListItem_Should_Pass()
        {
            // Instantiate a new ItemLocation Base, should have default of 1 for all values
            var myDataList = ItemLocationList.GetListItem;

            // Get Expected set
            var myList = Enum.GetNames(typeof(ItemLocationEnum)).ToList();
            var myExpectedList = myList.Where(a =>
                                            a.ToString() != ItemLocationEnum.Unknown.ToString() &&
                                            a.ToString() != ItemLocationEnum.LeftFinger.ToString() &&
                                            a.ToString() != ItemLocationEnum.RightFinger.ToString()
                                            )
                                            .OrderBy(a => a)
                                            .ToList();

            // Make sure each item is in the list
            foreach (var item in myDataList)
            {
                var found = false;
                foreach (var expected in myExpectedList)
                {
                    if (item == expected)
                    {
                        found = true;
                        break;
                    }
                }
                Assert.AreEqual(true, found, "item : " + item + TestContext.CurrentContext.Test.Name);
            }

            // reverse it, to make sure the list has each item
            // Make sure each item is in the list
            foreach (var expected in myExpectedList)
            {
                var found = false;
                {
                    foreach (var item in myDataList)
                        if (item == expected)
                        {
                            found = true;
                            break;
                        }
                }
                Assert.AreEqual(true, found, "expected : " + expected + TestContext.CurrentContext.Test.Name);
            }

        }

        [Test]
        public void Model_ItemLocationList_ConvertStringToEnum_Should_Pass()
        {
            var myList = Enum.GetNames(typeof(ItemLocationEnum)).ToList();

            ItemLocationEnum myActual;
            ItemLocationEnum myExpected;

            foreach (var item in myList)
            {
                myActual = ItemLocationList.ConvertStringToEnum(item);
                myExpected = (ItemLocationEnum)Enum.Parse(typeof(ItemLocationEnum), item);

                Assert.AreEqual(myExpected, myActual, "string: " + item + TestContext.CurrentContext.Test.Name);
            }
        }

        [Test]
        public void Model_ItemLocationList_GetLocationByPosition_1_Should_Pass()
        {
            var value = 1;
            var Actual = ItemLocationList.GetLocationByPosition(value);
            var Expected = ItemLocationEnum.Head;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void Model_ItemLocationList_GetLocationByPosition_2_Should_Pass()
        {
            var value = 2;
            var Actual = ItemLocationList.GetLocationByPosition(value);
            var Expected = ItemLocationEnum.Necklass;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void Model_ItemLocationList_GetLocationByPosition_3_Should_Pass()
        {
            var value = 3;
            var Actual = ItemLocationList.GetLocationByPosition(value);
            var Expected = ItemLocationEnum.PrimaryHand;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }
        
        [Test]
        public void Model_ItemLocationList_GetLocationByPosition_4_Should_Pass()
        {
            var value = 4;
            var Actual = ItemLocationList.GetLocationByPosition(value);
            var Expected = ItemLocationEnum.OffHand;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }


        [Test]
        public void Model_ItemLocationList_GetLocationByPosition_5_Should_Pass()
        {
            var value = 5;
            var Actual = ItemLocationList.GetLocationByPosition(value);
            var Expected = ItemLocationEnum.RightFinger;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void Model_ItemLocationList_GetLocationByPosition_6_Should_Pass()
        {
            var value = 6;
            var Actual = ItemLocationList.GetLocationByPosition(value);
            var Expected = ItemLocationEnum.LeftFinger;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void Model_ItemLocationList_GetLocationByPosition_7_Should_Pass()
        {
            var value = 7;
            var Actual = ItemLocationList.GetLocationByPosition(value);
            var Expected = ItemLocationEnum.Feet;

            Assert.AreEqual(Expected, Actual, TestContext.CurrentContext.Test.Name);
        }
    }
}

