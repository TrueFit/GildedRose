using GildedRose.Data;
using GildedRose.Services;
using NUnit.Framework;

namespace GildedRose.Tests
{
    [TestFixture]
    public class ConjuredItemsTests : ItemTestsBase
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
            Item = (ConjuredItem) Factory.CreateItem("Giant Slayer", "Conjured", 1, 49);

            // act - test after 1 days
            UpdateItemMultipleTimes(Item, 1);

            //assert
            Assert.AreEqual(Item.Name, "Giant Slayer");
            Assert.AreEqual(Item.Category, "Conjured");
            Assert.AreEqual(Item.SellIn, 0);
            Assert.AreEqual(Item.Quality, 47);
        }

        [Test]
        public void TestLowerSellInCondition()
        {
            // arrange
            Item = (ConjuredItem) Factory.CreateItem("Giant Slayer", "Conjured", 10, 10);

            // act - test Brie after 11 days
            UpdateItemMultipleTimes(Item, 11);

            //assert
            Assert.AreEqual(Item.Name, "Giant Slayer");
            Assert.AreEqual(Item.Category, "Conjured");
            Assert.AreEqual(Item.SellIn, -1);
            Assert.AreEqual(Item.Quality, 0);
        }

        [Test]
        public void TestStormHammer4Days()
        {

            // arrange
            Item = (ConjuredItem) Factory.CreateItem("Storm Hammer", "Conjured", 40, 10);

            // act  - test after 4 days
            UpdateItemMultipleTimes(Item, 4);

            //assert
            Assert.AreEqual(Item.Name, "Storm Hammer");
            Assert.AreEqual(Item.Category, "Conjured");
            Assert.AreEqual(Item.SellIn, 36);
            Assert.AreEqual(Item.Quality, 2);
        }
    }
}

