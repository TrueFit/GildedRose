using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GildedRose.Entities;

namespace GildedRose.Translators.Tests
{
    /// <summary>
    /// Yeah, these are pretty much cheeseball tests...
    /// Not really needed since they're testing properties of C#.
    /// </summary>
    [TestClass]
    public class GildedRoseTranslatorsTests
    {
        [TestMethod]
        public void TestTranslateSingleEntityToBusinessObject()
        {
            // Arrange
            var entityItem = new GildedRoseInventoryItem
            {
                id = 1,
                category = "category",
                name = "name",
                quality = 2,
                sellin = 3
            };

            // Act
            var item = entityItem.AsInventoryItem();

            // Assert
            Assert.AreEqual(1, item.Id);
            Assert.AreEqual("name", item.Name);
            Assert.AreEqual("category", item.Category);
            Assert.AreEqual(2, item.Quality);
            Assert.AreEqual(3, item.Sellin);
        }

        [TestMethod]
        public void TestTranslateMultipleEntitiesToBusinessObjects()
        {
            // Arrange
            var entityItem1 = new GildedRoseInventoryItem
            {
                id = 1,
                category = "category 1",
                name = "name 1",
                quality = 2,
                sellin = 3
            };
            var entityItem2 = new GildedRoseInventoryItem
            {
                id = 2,
                category = "category 2",
                name = "name 2",
                quality = 3,
                sellin = 4
            };
            var entityItems = new List<GildedRoseInventoryItem> {entityItem1, entityItem2};

            // Act
            var items = entityItems.AsInventoryItems().ToList();

            // Assert
            Assert.AreEqual(2, items.Count);

            Assert.AreEqual(1, items[0].Id);
            Assert.AreEqual("name 1", items[0].Name);
            Assert.AreEqual("category 1", items[0].Category);
            Assert.AreEqual(2, items[0].Quality);
            Assert.AreEqual(3, items[0].Sellin);

            Assert.AreEqual(2, items[1].Id);
            Assert.AreEqual("name 2", items[1].Name);
            Assert.AreEqual("category 2", items[1].Category);
            Assert.AreEqual(3, items[1].Quality);
            Assert.AreEqual(4, items[1].Sellin);
        }
    }
}
