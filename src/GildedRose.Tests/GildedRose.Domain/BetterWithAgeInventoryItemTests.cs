using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GildedRose.Domain.Tests
{
    [TestClass]
    public class BetterWithAgeInventoryItemTests
    {
        private static readonly Guid TestId = Guid.NewGuid();

        [TestMethod]
        public void BetterWithAgeInventoryItem_ctor()
        {
            var item = new BetterWithAgeInventoryItem(TestId, "name", "category", 5);
            Assert.AreEqual(TestId, item.Id);
            Assert.AreEqual("name", item.Name);
            Assert.AreEqual("category", item.Category);
            Assert.IsNull(item.SellIn);
            Assert.AreEqual(5, item.Quality);
        }

        [DataRow(5, 6)]
        [DataRow(49, 50)]
        [DataRow(50, 50)]
        [DataTestMethod]
        public void BetterWithAgeInventoryItem_OnAdvanceToNextDay(int beforeQuality, int afterQuality)
        {
            var item = new BetterWithAgeInventoryItem(TestId, "name", "category", beforeQuality);
            item.OnAdvanceToNextDay();
            Assert.AreEqual(afterQuality, item.Quality);
        }
    }
}