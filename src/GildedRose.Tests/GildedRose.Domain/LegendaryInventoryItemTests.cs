using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GildedRose.Domain.Tests
{
    [TestClass]
    public class LegendaryInventoryItemTests
    {
        private static readonly Guid TestId = Guid.NewGuid();

        [TestMethod]
        public void LegendaryInventoryItem_ctor()
        {
            var item = new LegendaryInventoryItem(TestId, "name", "category", 77);
            Assert.AreEqual(TestId, item.Id);
            Assert.AreEqual("name", item.Name);
            Assert.AreEqual("category", item.Category);
            Assert.IsNull(item.SellIn);
            Assert.AreEqual(77, item.Quality);
        }

        [DataRow(0)]
        [DataRow(77)]
        [DataRow(80)]
        [DataTestMethod]
        public void LegendaryInventoryItem_OnAdvanceToNextDay_QualityDoesntChange(int quality)
        {
            var item = new LegendaryInventoryItem(TestId, "name", "category", quality);
            item.OnAdvanceToNextDay();
            Assert.AreEqual(quality, item.Quality);
        }
    }
}