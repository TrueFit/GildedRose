using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GildedRose.Domain.Tests
{
    [TestClass]
    public class InventoryItemBuilderTests
    {
        private static readonly Guid TestId = Guid.NewGuid();

        [TestMethod]
        public void Build_AgedBrie()
        {
            var builder = new InventoryItemBuilder();
            IInventoryItem item = builder.Build(TestId, "Aged Brie", "Food", 50, 10);
            Assert.IsInstanceOfType(item, typeof(BetterWithAgeInventoryItem));
            Assert.AreEqual(TestId, item.Id);
            Assert.AreEqual("Aged Brie", item.Name);
            Assert.AreEqual("Food", item.Category);
            Assert.AreEqual(50, item.Quality);
            Assert.IsNull(item.SellIn);
        }

        [TestMethod]
        public void Build_BackstagePasses()
        {
            var builder = new InventoryItemBuilder();
            IInventoryItem item = builder.Build(TestId, "Raging Ogre", "Backstage Passes", 10, 10);
            Assert.IsInstanceOfType(item, typeof(BackstagePassInventoryItem));
            Assert.AreEqual(TestId, item.Id);
            Assert.AreEqual("Raging Ogre", item.Name);
            Assert.AreEqual("Backstage Passes", item.Category);
            Assert.AreEqual(10, item.Quality);
            Assert.AreEqual(10, item.SellIn);
        }

        [TestMethod]
        public void Build_Conjured()
        {
            var builder = new InventoryItemBuilder();
            IInventoryItem item = builder.Build(TestId, "Giant Slayer", "Conjured", 15, 50);
            Assert.IsInstanceOfType(item, typeof(ConjuredInventoryItem));
            Assert.AreEqual(TestId, item.Id);
            Assert.AreEqual("Giant Slayer", item.Name);
            Assert.AreEqual("Conjured", item.Category);
            Assert.AreEqual(15, item.Quality);
            Assert.AreEqual(50, item.SellIn);
        }

        [TestMethod]
        public void Build_StandardItem()
        {
            var builder = new InventoryItemBuilder();
            IInventoryItem item = builder.Build(TestId, "Sword", "Weapon", 50, 30);
            Assert.IsInstanceOfType(item, typeof(StandardInventoryItem));
            Assert.AreEqual(TestId, item.Id);
            Assert.AreEqual("Sword", item.Name);
            Assert.AreEqual("Weapon", item.Category);
            Assert.AreEqual(50, item.Quality);
            Assert.AreEqual(30, item.SellIn);
        }

        [TestMethod]
        public void Build_Sulfuras()
        {
            var builder = new InventoryItemBuilder();
            IInventoryItem item = builder.Build(TestId, "Hand of Ragnaros", "Sulfuras", 80, 80);
            Assert.IsInstanceOfType(item, typeof(LegendaryInventoryItem));
            Assert.AreEqual(TestId, item.Id);
            Assert.AreEqual("Hand of Ragnaros", item.Name);
            Assert.AreEqual("Sulfuras", item.Category);
            Assert.AreEqual(80, item.Quality);
            Assert.IsNull(item.SellIn);
        }
    }
}