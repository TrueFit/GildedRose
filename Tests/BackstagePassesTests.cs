using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GildedRose.Data;
using GildedRose.Services;
using NUnit.Framework;

namespace GildedRose.Tests
{
    class BackstagePassesTests : ItemTestsBase
    {
        [SetUp]
        public void Setup()
        {
            Factory = new ItemFactory();
        }

        [Test]
        public void TestBaseline()
        {
            // arrange
            Item = (BackstagePass)Factory.CreateItem("Raging Ogre", "Backstage Passes", 1, 49);

            // act - test pass after 1 days
            UpdateItemMultipleTimes(Item, 1);

            //assert
            Assert.AreEqual(Item.Name, "Raging Ogre");
            Assert.AreEqual(Item.Category, "Backstage Passes");
            Assert.AreEqual(Item.SellIn, 0);
            Assert.AreEqual(Item.Quality, 0);
        }

        [Test]
        public void Test5DayCondition01()
        {
            // arrange
            Item = (BackstagePass)Factory.CreateItem("Raging Ogre", "Backstage Passes", 5, 37);

            // act - test pass after 4 days
            UpdateItemMultipleTimes(Item, 4);

            //assert
            Assert.AreEqual(Item.Name, "Raging Ogre");
            Assert.AreEqual(Item.Category, "Backstage Passes");
            Assert.AreEqual(Item.SellIn, 1);
            Assert.AreEqual(Item.Quality, 49);
        }

        [Test]
        public void Test5DayCondition02()
        {
            // arrange
            Item = (BackstagePass)Factory.CreateItem("Raging Ogre", "Backstage Passes", 5, 37);

            // act - test pass after 4 days
            UpdateItemMultipleTimes(Item, 5);

            //assert
            Assert.AreEqual(Item.Name, "Raging Ogre");
            Assert.AreEqual(Item.Category, "Backstage Passes");
            Assert.AreEqual(Item.SellIn, 0);
            Assert.AreEqual(Item.Quality, 0);
        }

        [Test]
        public void Test5DayCondition03()
        {
            // arrange
            Item = (BackstagePass)Factory.CreateItem("Raging Ogre", "Backstage Passes", 5, 39);

            // act - test pass after 4 days
            UpdateItemMultipleTimes(Item, 4);

            //assert
            Assert.AreEqual(Item.Name, "Raging Ogre");
            Assert.AreEqual(Item.Category, "Backstage Passes");
            Assert.AreEqual(Item.SellIn, 1);
            Assert.AreEqual(Item.Quality, 50);
        }

        [Test]
        public void Test6DayCondition()
        {
            // arrange
            Item = (BackstagePass)Factory.CreateItem("Raging Ogre", "Backstage Passes", 6, 37);

            // act - test pass after 5 days
            UpdateItemMultipleTimes(Item, 5);

            //assert
            Assert.AreEqual(Item.Name, "Raging Ogre");
            Assert.AreEqual(Item.Category, "Backstage Passes");
            Assert.AreEqual(Item.SellIn, 1);
            Assert.AreEqual(Item.Quality, 50);
        }
        
        [Test]
        public void Test30DayCondition()
        {
            // arrange
            Item = (BackstagePass)Factory.CreateItem("Raging Ogre", "Backstage Passes", 31, 10);

            // act - test pass after 30 days
            UpdateItemMultipleTimes(Item, 30);

            //assert
            Assert.AreEqual(Item.Name, "Raging Ogre");
            Assert.AreEqual(Item.Category, "Backstage Passes");
            Assert.AreEqual(Item.SellIn, 1);
            Assert.AreEqual(Item.Quality, 50);
        }

        [Test]
        public void Test31DayCondition()
        {
            // arrange
            Item = (BackstagePass)Factory.CreateItem("Raging Ogre", "Backstage Passes", 31, 10);

            // act - test pass after 30 days
            UpdateItemMultipleTimes(Item, 31);

            //assert
            Assert.AreEqual(Item.Name, "Raging Ogre");
            Assert.AreEqual(Item.Category, "Backstage Passes");
            Assert.AreEqual(Item.SellIn, 0);
            Assert.AreEqual(Item.Quality, 0);
        }
    }

}
