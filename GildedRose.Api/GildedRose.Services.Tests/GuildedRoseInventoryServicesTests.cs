using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GildedRose.Repositories;
using Moq;
using Ploeh.AutoFixture;
using GildedRose.Entities;
using GildedRose.Services;

namespace GildedRose.Services.Tests
{
    [TestClass]
    public class GildedRoseInventoryServicesTests
    {   
        [TestMethod]
        public void ServicesAllItemInvokesGetAllItems()
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

            var itemsServiceMock = new Mock<GildedRoseEntities>();
            itemsServiceMock.Setup(x => x.GildedRoseInventoryItems).Returns(itemsMock.Object);

            var itemsService = new InventoryItemServices(itemsServiceMock.Object);

            // Act
            var findItems = itemsService.GetAllItems();

            // Assert
            Assert.AreEqual(3, findItems.Count());
        }

        [TestMethod]
        public void ServicesAllItemInvokesGetZeroQualityItems()
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

            var itemsServiceMock = new Mock<GildedRoseEntities>();
            itemsServiceMock.Setup(x => x.GildedRoseInventoryItems).Returns(itemsMock.Object);

            var itemsService = new InventoryItemServices(itemsServiceMock.Object);

            // Act
            var findItems = itemsService.GetZeroQualityItems();

            // Assert
            Assert.AreEqual(2, findItems.Count());
        }

        [TestMethod]
        public void ServicesFindInvokesFindByName()
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

            var itemsServiceMock = new Mock<GildedRoseEntities>();
            itemsServiceMock.Setup(x => x.GildedRoseInventoryItems).Returns(itemsMock.Object);

            var itemsService = new InventoryItemServices(itemsServiceMock.Object);

            // Act
            var findItem = itemsService.Find("name 2");

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
