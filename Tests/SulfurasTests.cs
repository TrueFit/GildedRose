using GildedRose.Data;
using GildedRose.Services;
using NUnit.Framework;

namespace GildedRose.Tests
{
    [TestFixture]
    public class SulfurasTests : ItemTestsBase
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
            Item = (Sulfuras) Factory.CreateItem("Hand of Ragnaros", "Sulfuras", 80, 80);

            // act - test Brie after 1 day
            UpdateItemMultipleTimes(Item, 1);

            //assert
            Assert.AreEqual(Item.Name, "Hand of Ragnaros");
            Assert.AreEqual(Item.Category, "Sulfuras");
            Assert.AreEqual(Item.SellIn, 80);
            Assert.AreEqual(Item.Quality, 80);
        }

        [Test]
        public void TestLowQualitySet()
        {
            // arrange
            Item = (Sulfuras) Factory.CreateItem("Hand of Ragnaros", "Sulfuras", 10, 10);

            // act - test Brie after 10 days
            UpdateItemMultipleTimes(Item, 10);

            //assert
            Assert.AreEqual(Item.Name, "Hand of Ragnaros");
            Assert.AreEqual(Item.Category, "Sulfuras");
            Assert.AreEqual(Item.SellIn, 10);
            Assert.AreEqual(Item.Quality, 80);
        }

        [Test]
        public void TestZeroQuality()
        {
            // arrange
            Item = (Sulfuras) Factory.CreateItem("Hand of Ragnaros", "Sulfuras", 0, 0);

            // act - test Brie after 41 days
            UpdateItemMultipleTimes(Item, 41);

            //assert
            Assert.AreEqual(Item.Name, "Hand of Ragnaros");
            Assert.AreEqual(Item.Category, "Sulfuras");
            Assert.AreEqual(Item.SellIn, 0);
            Assert.AreEqual(Item.Quality, 80);
        }
    }
}

