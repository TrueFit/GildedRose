using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GildedRose.Domain.Tests
{
    [TestClass]
    public class BackstagePassInventoryItemTests
    {
        private static readonly Guid TestId = Guid.NewGuid();

        [TestMethod]
        public void BackstagePassInventoryItemTest_ctor()
        {
            var item = new BackstagePassInventoryItem(TestId, "name", "category", 5, 3);
            Assert.AreEqual(TestId, item.Id);
            Assert.AreEqual("name", item.Name);
            Assert.AreEqual("category", item.Category);
            Assert.AreEqual(3, item.SellIn.Value);
            Assert.AreEqual(5, item.Quality);
        }

        [DataRow(20, 5, 19, 6)]
        [DataRow(12, 5, 11, 6)]
        [DataRow(11, 5, 10, 7)]
        [DataRow(10, 5, 9, 7)]
        [DataRow(7, 5, 6, 7)]
        [DataRow(6, 5, 5, 8)]
        [DataRow(5, 5, 4, 8)]
        [DataRow(2, 5, 1, 8)]
        [DataRow(1, 5, 0, 0)]
        [DataRow(0, 5, -1, 0)]
        [DataRow(-1, 5, -2, 0)]
        [DataTestMethod]
        public void BackstagePassInventoryItemTest_OnAdvanceToNextDay(int beforeSellIn, int beforeQuality, int afterSellIn, int afterQuality)
        {
            var item = new BackstagePassInventoryItem(TestId, "name", "category", beforeQuality, beforeSellIn);
            item.OnAdvanceToNextDay();
            Assert.AreEqual(afterSellIn, item.SellIn.Value);
            Assert.AreEqual(afterQuality, item.Quality);
        }
    }
}