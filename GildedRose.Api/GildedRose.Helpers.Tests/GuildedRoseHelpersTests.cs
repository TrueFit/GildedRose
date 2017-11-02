using Microsoft.VisualStudio.TestTools.UnitTesting;
using GildedRose.BusinessObjects;

namespace GildedRose.Helpers.Tests
{
    [TestClass]
    public class GildedRoseHelpersTests
    {
        [TestMethod]
        public void HelpersEndDayDefaultItem()
        {
            // Arrange
            var i = new InventoryItem
            {
                Id = 1,
                Name = "Test Item",
                Category = "blahblahblah",
                Sellin = 2,
                Quality = 5
            };

            // Act
            var changedItem = DayHelpers.EndDay(i);

            // Assert
            Assert.AreEqual(1, changedItem.Sellin);
            Assert.AreEqual(4, changedItem.Quality);
        }

        [TestMethod]
        public void HelpersEndDayDefaultSellinToZero()
        {
            // Arrange
            var i = new InventoryItem
            {
                Id = 1,
                Name = "Test Item",
                Category = "blahblahblah",
                Sellin = 1,
                Quality = 5
            };

            // Act
            var changedItem = DayHelpers.EndDay(i);

            // Assert
            Assert.AreEqual(0, changedItem.Sellin);
            Assert.AreEqual(3, changedItem.Quality);
        }

        [TestMethod]
        public void HelpersEndDayAgedBrieItem()
        {
            // Arrange
            var i = new InventoryItem
            {
                Id = 1,
                Name = "Aged Brie",
                Category = "food",
                Sellin = 2,
                Quality = 5
            };

            // Act
            var changedItem = DayHelpers.EndDay(i);

            // Assert
            Assert.AreEqual(1, changedItem.Sellin);
            Assert.AreEqual(6, changedItem.Quality);
        }

        [TestMethod]
        public void HelpersEndDayBackstagePassesOver10Days()
        {
            // Arrange
            var i = new InventoryItem
            {
                Id = 1,
                Name = "The Smiths Reunion Tour",
                Category = "Backstage Passes",
                Sellin = 12,
                Quality = 5
            };

            // Act
            var changedItem = DayHelpers.EndDay(i);

            // Assert
            Assert.AreEqual(11, changedItem.Sellin);
            Assert.AreEqual(6, changedItem.Quality);
        }

        [TestMethod]
        public void HelpersEndDayBackstagePassesUnder10Over5Days()
        {
            // Arrange
            var i = new InventoryItem
            {
                Id = 1,
                Name = "The Smiths Reunion Tour",
                Category = "Backstage Passes",
                Sellin = 9,
                Quality = 5
            };

            // Act
            var changedItem = DayHelpers.EndDay(i);

            // Assert
            Assert.AreEqual(8, changedItem.Sellin);
            Assert.AreEqual(7, changedItem.Quality);
        }

        [TestMethod]
        public void HelpersEndDayBackstagePassesUnder5Days()
        {
            // Arrange
            var i = new InventoryItem
            {
                Id = 1,
                Name = "The Smiths Reunion Tour",
                Category = "Backstage Passes",
                Sellin = 4,
                Quality = 5
            };

            // Act
            var changedItem = DayHelpers.EndDay(i);

            // Assert
            Assert.AreEqual(3, changedItem.Sellin);
            Assert.AreEqual(8, changedItem.Quality);
        }

        [TestMethod]
        public void HelpersEndDayBackstagePassesConcertOver()
        {
            // Arrange
            var i = new InventoryItem
            {
                Id = 1,
                Name = "The Smiths Reunion Tour",
                Category = "Backstage Passes",
                Sellin = 1,
                Quality = 5
            };

            // Act
            var changedItem = DayHelpers.EndDay(i);

            // Assert
            Assert.AreEqual(0, changedItem.Sellin);
            Assert.AreEqual(0, changedItem.Quality);
        }

        [TestMethod]
        public void HelpersEndDayConjured()
        {
            // Arrange
            var i = new InventoryItem
            {
                Id = 1,
                Name = "Conjured Test",
                Category = "Conjured",
                Sellin = 10,
                Quality = 10
            };

            // Act
            var changedItem = DayHelpers.EndDay(i);

            // Assert
            Assert.AreEqual(9, changedItem.Sellin);
            Assert.AreEqual(8, changedItem.Quality);
        }

        [TestMethod]
        public void HelpersEndDaySulfuras()
        {
            // Arrange
            var i = new InventoryItem
            {
                Id = 1,
                Name = "Sulfuras Test",
                Category = "Sulfuras",
                Sellin = 22,
                Quality = 33
            };

            // Act
            var changedItem = DayHelpers.EndDay(i);

            // Assert
            Assert.AreEqual(80, changedItem.Sellin);
            Assert.AreEqual(80, changedItem.Quality);
        }
    }
}
