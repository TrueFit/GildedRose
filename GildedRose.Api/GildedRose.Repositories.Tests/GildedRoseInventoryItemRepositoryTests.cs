using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using Castle.Core.Internal;
using GildedRose.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GildedRose.Repositories;
using Ploeh.AutoFixture;

namespace GildedRose.Repositories.Tests
{
    [TestClass]
    public class GildedRoseInventoryItemRepositoryTests
    {
        [TestMethod]
        public void RepositoryAddItemInvokesAddNoLinq()
        {
            // Arrange
            var itemContextMock = new Mock<GildedRoseEntities>();
            itemContextMock.Setup(x => x.GildedRoseInventoryItems.Add(It.IsAny<GildedRoseInventoryItem>())).Returns((GildedRoseInventoryItem u) => u);

            var itemRepo = new GildedRoseInventoryItemRepository(itemContextMock.Object);
            var dummyItem = new GildedRoseInventoryItem { category = "category", name = "name", sellin = 1, quality = 2 };

            // Act
            var itemToTest = itemRepo.Add(dummyItem);

            // Assert
            Assert.AreEqual("category", itemToTest.category);
            Assert.AreEqual("name", itemToTest.name);
            Assert.AreEqual(1, itemToTest.sellin);
            Assert.AreEqual(2, itemToTest.quality);
        }
        [TestMethod]
        public void RepositoryFindItemInvokesFindById()
        {
            // Arrange
            var fixture = new Fixture();
            IList<GildedRoseInventoryItem> items = new List<GildedRoseInventoryItem>
            {
                fixture.Build<GildedRoseInventoryItem>().With(u => u.id, 1).Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.id, 2).Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.id, 3).Create()
            };

            var itemsMock = CreateDbSetMock(items);

            var itemsRepoMock = new Mock<GildedRoseEntities>();
            itemsRepoMock.Setup(x => x.GildedRoseInventoryItems).Returns(itemsMock.Object);

            var itemsRepo = new GildedRoseInventoryItemRepository(itemsRepoMock.Object);

            // Act
            var findItem = itemsRepo.Find(1);

            // Assert
            Assert.AreEqual(1, findItem.id);
        }

        [TestMethod]
        public void RepositoryFindItemInvokesFindByName()
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

            var itemsRepoMock = new Mock<GildedRoseEntities>();
            itemsRepoMock.Setup(x => x.GildedRoseInventoryItems).Returns(itemsMock.Object);

            var itemsRepo = new GildedRoseInventoryItemRepository(itemsRepoMock.Object);

            // Act
            var findItem = itemsRepo.Find("name 2");

            // Assert
            Assert.AreEqual("name 2", findItem.name);
        }

        [TestMethod]
        public void RepositoryAllItemInvokesGetAllItems()
        {
            // Arrange
            var fixture = new Fixture();
            IList<GildedRoseInventoryItem> items = new List<GildedRoseInventoryItem>
            {
                fixture.Build<GildedRoseInventoryItem>().With(u => u.name, "name 1").With(u => u.id, 1).With(u => u.category, "cat 1").Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.name, "name 2").Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.name, "name 3").Create()
            };

            var itemsMock = CreateDbSetMock(items);

            var itemsRepoMock = new Mock<GildedRoseEntities>();
            itemsRepoMock.Setup(x => x.GildedRoseInventoryItems).Returns(itemsMock.Object);

            var itemsRepo = new GildedRoseInventoryItemRepository(itemsRepoMock.Object);

            // Act
            var findItems = itemsRepo.GetAllItems();

            // Assert
            Assert.AreEqual(3, findItems.Count());
        }

        [TestMethod]
        public void RepositoryAllItemInvokesGetZeroQualityItems()
        {
            // Arrange
            var fixture = new Fixture();
            IList<GildedRoseInventoryItem> items = new List<GildedRoseInventoryItem>
            {
                fixture.Build<GildedRoseInventoryItem>().With(u => u.quality, 0).Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.quality, 2).Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.quality, 0).Create()
            };

            var itemsMock = CreateDbSetMock(items);

            var itemsRepoMock = new Mock<GildedRoseEntities>();
            itemsRepoMock.Setup(x => x.GildedRoseInventoryItems).Returns(itemsMock.Object);

            var itemsRepo = new GildedRoseInventoryItemRepository(itemsRepoMock.Object);

            // Act
            var findItems = itemsRepo.GetZeroQualityItems();

            // Assert
            Assert.AreEqual(2, findItems.Count());
        }

        [TestMethod]
        public void RepositoryAllItemInvokesRemoveItems()
        {
            // Arrange
            var fixture = new Fixture();
            IList<GildedRoseInventoryItem> items = new List<GildedRoseInventoryItem>
            {
                fixture.Build<GildedRoseInventoryItem>().With(u => u.id, 1).Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.id, 2).Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.id, 3).Create()
            };

            var itemsMock = CreateDbSetMock(items);

            var itemsRepoMock = new Mock<GildedRoseEntities>();
            itemsRepoMock.Setup(x => x.GildedRoseInventoryItems).Returns(itemsMock.Object);

            var itemsRepo = new GildedRoseInventoryItemRepository(itemsRepoMock.Object);

            // Act
            var testItem = itemsRepo.Remove(1);

            // Assert
            Assert.AreEqual(1, testItem.id);
        }

        [TestMethod]
        public void RepositoryAllItemInvokesUpdateItems()
        {
            // Arrange
            var fixture = new Fixture();
            IList<GildedRoseInventoryItem> items = new List<GildedRoseInventoryItem>
            {
                fixture.Build<GildedRoseInventoryItem>().With(u => u.id, 1).Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.id, 2).Create(),
                fixture.Build<GildedRoseInventoryItem>().With(u => u.id, 3).Create()
            };

            var itemsMock = CreateDbSetMock(items);

            var itemsRepoMock = new Mock<GildedRoseEntities>();
            itemsRepoMock.Setup(x => x.GildedRoseInventoryItems).Returns(itemsMock.Object);

            var itemsRepo = new GildedRoseInventoryItemRepository(itemsRepoMock.Object);

            // Act
            var testItem = itemsRepo.Update(2, 7, 9);

            // Assert
            Assert.AreEqual(2, testItem.id);
            Assert.AreEqual(9, testItem.sellin);
            Assert.AreEqual(7, testItem.quality);
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
