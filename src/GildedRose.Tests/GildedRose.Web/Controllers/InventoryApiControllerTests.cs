using GildedRose.Domain;
using GildedRose.Repository;
using GildedRose.Web.Models.InventoryApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRose.Web.Controllers.Tests
{
    [TestClass]
    public class InventoryApiControllerTests
    {
        private Mock<IInventoryItemBuilder> builder;
        private InventoryApiController controller;
        private Mock<IInventoryRepository> repository;

        [TestMethod]
        public async Task Add_Test()
        {
            var item = new Mock<IInventoryItem>();

            this.builder.Setup(_ => _.Build(It.IsAny<Guid>(), "name", "category", 3, 5)).Returns(item.Object);
            this.repository.Setup(_ => _.AddItemAsync(item.Object)).Verifiable();

            var model = new AddModel
            {
                Category = "category",
                Name = "name",
                Quality = 3,
                SellIn = 5
            };

            JsonResult result = (await this.controller.AddItem(model)) as JsonResult;
            IInventoryItem actualData = result.Value as IInventoryItem;

            Assert.AreSame(item.Object, actualData);

            this.repository.Verify();
        }

        [TestMethod]
        public async Task GetAll_Found()
        {
            var data = new[]
            {
                new StandardInventoryItem(Guid.NewGuid(), "name_1", "category_1", 3, 5),
                new StandardInventoryItem(Guid.NewGuid(), "name_2", "category_2", 7, 11)
            };

            this.repository.Setup(_ => _.GetAllAsync()).Returns(Task.FromResult(data.AsEnumerable<IInventoryItem>()));

            JsonResult result = (await this.controller.GetAll()) as JsonResult;
            IInventoryItem[] actualData = result.Value as IInventoryItem[];

            Assert.AreEqual(2, actualData.Length);
            CollectionAssert.AreEquivalent(data, actualData);
        }

        [TestMethod]
        public async Task GetAll_None()
        {
            this.repository.Setup(_ => _.GetAllAsync()).Returns(Task.FromResult(Enumerable.Empty<IInventoryItem>()));

            JsonResult result = (await this.controller.GetAll()) as JsonResult;
            IInventoryItem[] actualData = result.Value as IInventoryItem[];

            Assert.AreEqual(0, actualData.Length);
        }

        [TestMethod]
        public async Task GetByName_MultipleResults()
        {
            var data = new[]
            {
                new StandardInventoryItem(Guid.NewGuid(), "name_1", "category_1", 3, 5),
                new StandardInventoryItem(Guid.NewGuid(), "name_2", "category_2", 7, 11),
                new StandardInventoryItem(Guid.NewGuid(), "name_2", "category_3", 13, 17)
            };

            this.repository.Setup(_ => _.GetAllAsync()).Returns(Task.FromResult(data.AsEnumerable<IInventoryItem>()));

            JsonResult result = (await this.controller.GetByName("name_2")) as JsonResult;
            IInventoryItem[] actualData = result.Value as IInventoryItem[];

            Assert.AreEqual(2, actualData.Length);
            Assert.AreEqual(data[1].Id, actualData[0].Id);
            Assert.AreEqual(data[2].Id, actualData[1].Id);
            Assert.AreEqual("name_2", actualData[0].Name);
            Assert.AreEqual("name_2", actualData[1].Name);
        }

        [TestMethod]
        public async Task GetByName_NotFound()
        {
            var data = new[]
            {
                new StandardInventoryItem(Guid.NewGuid(), "name_1", "category_1", 3, 5),
                new StandardInventoryItem(Guid.NewGuid(), "name_2", "category_2", 7, 11)
            };

            this.repository.Setup(_ => _.GetAllAsync()).Returns(Task.FromResult(data.AsEnumerable<IInventoryItem>()));

            JsonResult result = (await this.controller.GetByName("name_3")) as JsonResult;
            IInventoryItem[] actualData = result.Value as IInventoryItem[];

            Assert.AreEqual(0, actualData.Length);
        }

        [TestMethod]
        public async Task GetByName_SingleResult()
        {
            var data = new[]
            {
                new StandardInventoryItem(Guid.NewGuid(), "name_1", "category_1", 3, 5),
                new StandardInventoryItem(Guid.NewGuid(), "name_2", "category_2", 7, 11)
            };

            this.repository.Setup(_ => _.GetAllAsync()).Returns(Task.FromResult(data.AsEnumerable<IInventoryItem>()));

            JsonResult result = (await this.controller.GetByName("name_2")) as JsonResult;
            IInventoryItem[] actualData = result.Value as IInventoryItem[];

            Assert.AreEqual(1, actualData.Length);
            Assert.AreEqual(data[1].Id, actualData[0].Id);
            Assert.AreEqual("name_2", actualData[0].Name);
        }

        [TestMethod]
        public async Task GetTrash_Found()
        {
            var data = new[]
            {
                new StandardInventoryItem(Guid.NewGuid(), "name_1", "category_1", 3, 5),
                new StandardInventoryItem(Guid.NewGuid(), "name_2", "category_2", 0, 11)
            };

            this.repository.Setup(_ => _.GetAllAsync()).Returns(Task.FromResult(data.AsEnumerable<IInventoryItem>()));

            JsonResult result = (await this.controller.GetTrash()) as JsonResult;
            IInventoryItem[] actualData = result.Value as IInventoryItem[];

            Assert.AreEqual(1, actualData.Length);
            Assert.AreEqual(data[1].Id, actualData[0].Id);
            Assert.AreEqual(0, actualData[0].Quality);
        }

        [TestMethod]
        public async Task GetTrash_None()
        {
            var data = new[]
            {
                new StandardInventoryItem(Guid.NewGuid(), "name_1", "category_1", 3, 5),
                new StandardInventoryItem(Guid.NewGuid(), "name_2", "category_2", 7, 11)
            };

            this.repository.Setup(_ => _.GetAllAsync()).Returns(Task.FromResult(data.AsEnumerable<IInventoryItem>()));

            JsonResult result = (await this.controller.GetTrash()) as JsonResult;
            IInventoryItem[] actualData = result.Value as IInventoryItem[];

            Assert.AreEqual(0, actualData.Length);
        }

        [TestMethod]
        public async Task NextDay_Test()
        {
            var mock1 = new Mock<IInventoryItem>(MockBehavior.Strict);
            mock1.Setup(_ => _.OnAdvanceToNextDay()).Verifiable();
            mock1.Setup(_ => _.Name).Returns("name_1");
            mock1.Setup(_ => _.SellIn).Returns(0);

            var mock2 = new Mock<IInventoryItem>(MockBehavior.Strict);
            mock2.Setup(_ => _.OnAdvanceToNextDay()).Verifiable();
            mock2.Setup(_ => _.Name).Returns("name_2");
            mock2.Setup(_ => _.SellIn).Returns(0);

            IEnumerable<IInventoryItem> data = new[] { mock1.Object, mock2.Object };

            this.repository.Setup(_ => _.GetAllAsync()).Returns(Task.FromResult(data));
            this.repository.Setup(_ => _.SaveAsync(data)).Verifiable();

            JsonResult result = (await this.controller.NextDay()) as JsonResult;
            IInventoryItem[] actualData = result.Value as IInventoryItem[];

            Assert.AreEqual(2, actualData.Length);
            Assert.AreSame(mock1.Object, actualData[0]);
            Assert.AreSame(mock2.Object, actualData[1]);

            mock1.Verify();
            mock2.Verify();
            this.repository.Verify();
        }

        [TestMethod]
        public async Task Remove_Found()
        {
            Guid id = Guid.NewGuid();
            this.repository.Setup(_ => _.RemoveItemAsync(id)).Returns(Task.FromResult(true)).Verifiable();
            var result = await this.controller.RemoveItem(id);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            this.repository.Verify();
        }

        [TestMethod]
        public async Task Remove_NotFound()
        {
            Guid id = Guid.NewGuid();
            this.repository.Setup(_ => _.RemoveItemAsync(id)).Returns(Task.FromResult(false)).Verifiable();
            var result = await this.controller.RemoveItem(id);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            this.repository.Verify();
        }

        [TestMethod]
        public async Task Reset_Test()
        {
            this.repository.Setup(_ => _.ResetAsync()).Returns(Task.CompletedTask).Verifiable();
            var result = await this.controller.Reset();
            Assert.IsInstanceOfType(result, typeof(OkResult));
            this.repository.Verify();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.repository = new Mock<IInventoryRepository>();
            this.builder = new Mock<IInventoryItemBuilder>();
            this.controller = new InventoryApiController(this.builder.Object, this.repository.Object);
        }
    }
}