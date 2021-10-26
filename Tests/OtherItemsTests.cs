using System;
using System.Collections.Generic;
using GildedRose.Data;
using GildedRose.Services;
using NUnit.Framework;

namespace GildedRose.Tests
{
    [TestFixture]
    public class OtherItemsTests : ItemTestsBase
    {
        [SetUp]
        public void Setup()
        {
            Factory = new ItemFactory();
        }

        [Test]
        public void TestSword11Days()
        {
            // arrange
            Item = (Item) Factory.CreateItem("Sword", "Weapon", 30, 50);
            
            // act - test  after 11 days
            UpdateItemMultipleTimes(Item, 11);

            //assert
            Assert.AreEqual(Item.Name, "Sword");
            Assert.AreEqual(Item.Category, "Weapon");
            Assert.AreEqual(Item.SellIn, 19);
            Assert.AreEqual(Item.Quality, 39);
        }

        [Test]
        public void TestAxe35Days()
        {
            // arrange
            Item = (Item) Factory.CreateItem("Axe", "Weapon", 40, 50);
            
            // act - test after 35 days
            UpdateItemMultipleTimes(Item, 35);

            //assert
            Assert.AreEqual(Item.Name, "Axe");
            Assert.AreEqual(Item.Category, "Weapon");
            Assert.AreEqual(Item.SellIn, 5);
            Assert.AreEqual(Item.Quality, 15);
        }

        [Test]
        public void TestHalberd41Days()
        {
            // arrange
            Item = (Item) Factory.CreateItem("Halberd", "Weapon", 60, 40);
            
            // act - test after 41 days
            UpdateItemMultipleTimes(Item, 41);

            //assert
            Assert.AreEqual(Item.Name, "Halberd");
            Assert.AreEqual(Item.Category, "Weapon");
            Assert.AreEqual(Item.SellIn, 19);
            Assert.AreEqual(Item.Quality, 0);
        }

        [Test]
        public void TestMutton10days()
        {
            // arrange
            Item = (Item) Factory.CreateItem("Mutton", "Food", 10, 10);
            
            // act - test after 10 days
            UpdateItemMultipleTimes(Item, 10);

            //assert
            Assert.AreEqual(Item.Name, "Mutton");
            Assert.AreEqual(Item.Category, "Food");
            Assert.AreEqual(Item.SellIn, 0);
            Assert.AreEqual(Item.Quality, 0);
        }

        [Test]
        public void TestCheeseOneDay()
        {
            // arrange
            Item = (Item) Factory.CreateItem("Cheese", "Food", 5, 5);
            
            // act - test after 1 days
            UpdateItemMultipleTimes(Item, 1);

            //assert
            Assert.AreEqual(Item.Name, "Cheese");
            Assert.AreEqual(Item.Category, "Food");
            Assert.AreEqual(Item.SellIn, 4);
            Assert.AreEqual(Item.Quality, 4);
        }

        [Test]
        public void TestPotions5days()
        {
            // arrange
            Item = (Item) Factory.CreateItem("Potion of Healing", "Potion", 10, 10);
            
            // act - test after 5 days
            UpdateItemMultipleTimes(Item, 5);

            //assert
            Assert.AreEqual(Item.Name, "Potion of Healing");
            Assert.AreEqual(Item.Category, "Potion");
            Assert.AreEqual(Item.SellIn, 5);
            Assert.AreEqual(Item.Quality, 5);
        }

        [Test]
        public void TestHigherSellInCondition()
        {
           // arrange
            Item = (Item) Factory.CreateItem("Bag of Holding", "Misc", 10, 50);
            
            // act - test after 15 days
            UpdateItemMultipleTimes(Item, 15);

            //assert
            Assert.AreEqual(Item.Name, "Bag of Holding");
            Assert.AreEqual(Item.Category, "Misc");
            Assert.AreEqual(Item.SellIn, -5);
            Assert.AreEqual(Item.Quality, 30);
        }


        [Test]
        public void TestMongoose4days()
        {
            // arrange
            Item = (Item)Factory.CreateItem("Elixir of the Mongoose", "Potion", 5, 7);

            // act - test after 4 days
            UpdateItemMultipleTimes(Item, 4);

            //assert
            Assert.AreEqual(Item.Name, "Elixir of the Mongoose");
            Assert.AreEqual(Item.Category, "Potion");
            Assert.AreEqual(Item.SellIn, 1);
            Assert.AreEqual(Item.Quality, 3);
        }

        [Test]
        public void TestVest15days()
        {
            // arrange
            Item = (Item) Factory.CreateItem("+5 Dexterity Vest", "Armor", 10, 20);
            
            // act - test after 15 days
            UpdateItemMultipleTimes(Item, 15);

            //assert
            Assert.AreEqual(Item.Name, "+5 Dexterity Vest");
            Assert.AreEqual(Item.Category, "Armor");
            Assert.AreEqual(Item.SellIn, -5);
            Assert.AreEqual(Item.Quality, 0);
        }

        [Test]
        public void TestMail25days()
        {
            // arrange
            Item = (Item) Factory.CreateItem("Full Plate Mail", "Armor", 50, 50);
            
            // act - test after 25 days
            UpdateItemMultipleTimes(Item, 25);

            //assert
            Assert.AreEqual(Item.Name, "Full Plate Mail");
            Assert.AreEqual(Item.Category, "Armor");
            Assert.AreEqual(Item.SellIn, 25);
            Assert.AreEqual(Item.Quality, 25);
        }

        [Test]
        public void TestShield12Days()
        {
            // arrange
            Item = (Item) Factory.CreateItem("Wooden Shield", "Armor", 10, 30);
            
            // act - test after 12 days
            UpdateItemMultipleTimes(Item, 12);

            //assert
            Assert.AreEqual(Item.Name, "Wooden Shield");
            Assert.AreEqual(Item.Category, "Armor");
            Assert.AreEqual(Item.SellIn, -2);
            Assert.AreEqual(Item.Quality, 16);
        }
        [Test]
        public void TestUnknownCategory1Day()
        {
            // arrange
            try
            {
                Item = (Item) Factory.CreateItem("Thorny Crown", "Unknown", 10, 30);
                // act - test after 1 days
                UpdateItemMultipleTimes(Item, 1);

                //assert
                Assert.AreEqual(Item.Name, "Thorny Crown");
                Assert.AreEqual(Item.Category, "Unknown");
                Assert.AreEqual(Item.SellIn, -2);
                Assert.AreEqual(Item.Quality, 16);
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }
    }
}

