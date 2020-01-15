using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GildedRose.Domain.Tests
{
    [TestClass]
    public class StandardInventoryItemTests
    {
        private static readonly Guid TestId = Guid.NewGuid();

        [TestMethod]
        public void StandardInventoryItem_ctor()
        {
            var item = new StandardInventoryItem(TestId, "name", "category", 5, 3);
            Assert.AreEqual(TestId, item.Id);
            Assert.AreEqual("name", item.Name);
            Assert.AreEqual("category", item.Category);
            Assert.AreEqual(3, item.SellIn.Value);
            Assert.AreEqual(5, item.Quality);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StandardInventoryItem_ctor_EmptyCategoryThrowsException()
        {
            var _ = new StandardInventoryItem(TestId, "name", null, 5, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StandardInventoryItem_ctor_EmptyNameThrowsException()
        {
            var _ = new StandardInventoryItem(TestId, string.Empty, "category", 5, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StandardInventoryItem_ctor_QualityTooHighThrowsException()
        {
            var _ = new StandardInventoryItem(TestId, "name", "category", 51, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StandardInventoryItem_ctor_QualityTooLowThrowsException()
        {
            var _ = new StandardInventoryItem(TestId, "name", "category", -1, 3);
        }

        [DataRow(3, 5, 2, 4)]
        [DataRow(3, 1, 2, 0)]
        [DataRow(3, 0, 2, 0)]
        [DataRow(2, 5, 1, 4)]
        [DataRow(1, 5, 0, 3)]
        [DataRow(0, 5, -1, 3)]
        [DataRow(-1, 5, -2, 3)]
        [DataTestMethod]
        public void StandardInventoryItem_OnAdvanceToNextDay(int beforeSellIn, int beforeQuality, int afterSellIn, int afterQuality)
        {
            var item = new StandardInventoryItem(TestId, "name", "category", beforeQuality, beforeSellIn);
            item.OnAdvanceToNextDay();
            Assert.AreEqual(afterSellIn, item.SellIn.Value);
            Assert.AreEqual(afterQuality, item.Quality);
        }
    }
}