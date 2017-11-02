using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GildedRose.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using GildedRose.Api.Controllers;

namespace GildedRose.Api.Tests.Controllers
{
    [TestClass]
    public class InventoryItemControllerTests
    {
        [TestMethod]
        public void ItemControllerAllItemInvokesGetAllItems()
        {
            // Arrange
            var fixture = new Fixture();
            IList<GildedRoseInventoryItem> items = new List<GildedRoseInventoryItem>
            {
                fixture.Build<GildedRoseInventoryItem>().With(u => u.name, "name 1").Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.name, "name 2").Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.name, "name 3").Create()
            };

            var itemsMock = CreateDbSetMock(items);

            var itemsControllerMock = new Mock<GildedRoseEntities>();
            itemsControllerMock.Setup(x => x.GildedRoseInventoryItems).Returns(itemsMock.Object);

            var itemsController = new InventoryItemsController(itemsControllerMock.Object);

            // Act
            var findItems = itemsController.Get();

            // Assert
            Assert.AreEqual(3, findItems.Count());
        }

        [TestMethod]
        public void ItemControllerAllItemInvokesGetZeroQualityItems()
        {
            // Arrange
            var fixture = new Fixture();
            IList<GildedRoseInventoryItem> items = new List<GildedRoseInventoryItem>
            {
                fixture.Build<GildedRoseInventoryItem>().With(u => u.quality, 0).Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.quality, 1).Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.quality, 0).Create()
            };

            var itemsMock = CreateDbSetMock(items);

            var itemsControllerMock = new Mock<GildedRoseEntities>();
            itemsControllerMock.Setup(x => x.GildedRoseInventoryItems).Returns(itemsMock.Object);

            var itemsController = new InventoryItemsController(itemsControllerMock.Object);

            // Act
            var findItems = itemsController.GetZeroItems();

            // Assert
            Assert.AreEqual(2, findItems.Count());
        }

        [TestMethod]
        public void ItemControllerFindInvokesFindByName()
        {
            // Arrange
            var fixture = new Fixture();
            IList<GildedRoseInventoryItem> items = new List<GildedRoseInventoryItem>
            {
                fixture.Build<GildedRoseInventoryItem>().With(u => u.name, "name 1").Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.name, "name 2").Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.name, "name 3").Create()
            };

            var itemsMock = CreateDbSetMock(items);

            var itemsControllerMock = new Mock<GildedRoseEntities>();
            itemsControllerMock.Setup(x => x.GildedRoseInventoryItems).Returns(itemsMock.Object);

            var itemsController = new InventoryItemsController(itemsControllerMock.Object);

            // Act
            var findItem = itemsController.Get("name 2");

            // Assert
            Assert.AreEqual("name 2", findItem.Name);
        }

        private static Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class
        {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

            return dbSetMock;
        }
    }
}
