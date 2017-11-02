using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GildedRose.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;

namespace GildedRose.Services.Tests
{
    [TestClass]
    public class GildedRoseDayServicesTests
    {
        [TestMethod]
        public void ServicesFindInvokesFindByName()
        {
            // Arrange
            var fixture = new Fixture();
            IList<GildedRoseInventoryItem> items = new List<GildedRoseInventoryItem>
            {
                fixture.Build<GildedRoseInventoryItem>()
                .With(u => u.id, 1)
                .With(u => u.name, "Nothing")
                .With(u => u.category, "Stuff")
                .With(u => u.sellin, 3)
                .With(u => u.quality, 5)
                .Create(),
                fixture.Build<GildedRoseInventoryItem>()
                .With(u => u.id, 2)
                .With(u => u.name, "Aged Brie")
                .With(u => u.category, "Food")
                .With(u => u.sellin, 3)
                .With(u => u.quality, 5)
                .Create(),
                fixture.Build<GildedRoseInventoryItem>()
                .With(u => u.id, 3)
                .With(u => u.name, "The Cure")
                .With(u => u.category, "Backstage Passes")
                .With(u => u.sellin, 10)
                .With(u => u.quality, 1)
                .Create(),
                fixture.Build<GildedRoseInventoryItem>()
                .With(u => u.id, 4)
                .With(u => u.name, "Genie bottle with 1 wish")
                .With(u => u.category, "Conjured")
                .With(u => u.sellin, 5)
                .With(u => u.quality, 10)
                .Create(),
                fixture.Build<GildedRoseInventoryItem>()
                .With(u => u.id, 4)
                .With(u => u.name, "Holy Sword of Paladin Jeffrey")
                .With(u => u.category, "Sulfuras")
                .With(u => u.sellin, 5)
                .With(u => u.quality, 10)
                .Create(),
            };

            var itemsMock = CreateDbSetMock(items);

            var itemsServiceMock = new Mock<GildedRoseEntities>();
            itemsServiceMock.Setup(x => x.GildedRoseInventoryItems).Returns(itemsMock.Object);

            var itemsService = new InventoryDayServices(itemsServiceMock.Object);

            // Act
            var findItems = itemsService.IncrementDay(1).ToList();

            // Assert -- only doing basic business logic testing here, 
            // since helpers does the heavy lifting.
            
            // Default business logic
            Assert.AreEqual(2, findItems[0].Sellin);
            Assert.AreEqual(4, findItems[0].Quality);

            // Brie business logic
            Assert.AreEqual("Aged Brie", findItems[1].Name);
            Assert.AreEqual(2, findItems[1].Sellin);
            Assert.AreEqual(6, findItems[1].Quality);

            // Backstage Passes Business Logic
            Assert.AreEqual(9, findItems[2].Sellin);
            Assert.AreEqual(3, findItems[2].Quality);

            // Conjured Passes Business Logic
            Assert.AreEqual(4, findItems[3].Sellin);
            Assert.AreEqual(8, findItems[3].Quality);

            // Sulfuras Passes Business Logic
            Assert.AreEqual(80, findItems[4].Sellin);
            Assert.AreEqual(80, findItems[4].Quality);

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
