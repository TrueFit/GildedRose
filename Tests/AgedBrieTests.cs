using GildedRose.Data;
using GildedRose.Services;
using GildedRose.Tests;
using NUnit.Framework;

namespace GildedRose.Tests
{
    [TestFixture]
    public class AgedBrieTests : ItemTestsBase
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
            Item = (AgedBrie) Factory.CreateItem("Aged Brie", "Food", 1, 49);

            // act - test Brie after 11 days
            UpdateItemMultipleTimes(Item, 1);

            //assert
            Assert.AreEqual(Item.Name, "Aged Brie");
            Assert.AreEqual(Item.Category, "Food");
            Assert.AreEqual(Item.SellIn, 0);
            Assert.AreEqual(Item.Quality, 50);
        }

        [Test]
        public void TestLowerSellInCondition()
        {
            // arrange
            Item = (AgedBrie) Factory.CreateItem("Aged Brie", "Food", 10, 10);

            // act - test Brie after 11 days
            UpdateItemMultipleTimes(Item, 11);

            //assert
            Assert.AreEqual(Item.Name, "Aged Brie");
            Assert.AreEqual(Item.Category, "Food");
            Assert.AreEqual(Item.SellIn, -1);
            Assert.AreEqual(Item.Quality, 21);
        }

        [Test]
        public void TestHigherSellInCondition()
        {
            // test Brie after 41 days
            // arrange
            Item = (AgedBrie) Factory.CreateItem("Aged Brie", "Food", 40, 10);

            // act
            UpdateItemMultipleTimes(Item, 41);

            //assert
            Assert.AreEqual(Item.Name, "Aged Brie");
            Assert.AreEqual(Item.Category, "Food");
            Assert.AreEqual(Item.SellIn, -1);
            Assert.AreEqual(Item.Quality, 50);
        }
    }
}

