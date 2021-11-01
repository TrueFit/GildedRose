using DataAccessLibrary;
using GildedRose.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Test_GildedRose
{
    public class InventoryControllerTests
    {
        InventoryController _invController;
        CategoryController _catController;
        DayController _dayController;
        InventoryContext _service;

        public InventoryControllerTests()
        {
            _service = new InventoryContext();
            _invController = new InventoryController(_service);
            _catController = new CategoryController(_service);
            _dayController = new DayController(_service);
            _ = _invController.RepopulateDataToDefault();
        }

        [Fact]
        public void RePopulateAllDataTest()
        {
            var result = _invController.RepopulateDataToDefault();
            Assert.IsType<OkResult>(result.Result);
        }

        [Fact]
        public void GetAllItemsTest()
        {
            var result = _invController.GetAllItems();
            Assert.IsType<ActionResult<List<Item>>>(result.Result);
            var list = result.Result as ActionResult<List<Item>>;
            Assert.IsType<List<Item>>(list.Value);
            var listItems = list.Value as List<Item>;
            Assert.Equal(20, listItems.Count);
        }
        [Fact]
        public void GetAllCategoriesTest()
        {
            var result = _catController.GetAllCategories();
            Assert.IsType<ActionResult<List<Category>>>(result.Result);
            var list = result.Result as ActionResult<List<Category>>;
            Assert.IsType<List<Category>>(list.Value);
            var listCategories = list.Value as List<Category>;
            Assert.Equal(8, listCategories.Count);
        }
        [Fact]
        public void TestDayIncrement()
        {
            //repopulate for clean data and verify
            var repopulate = _invController.RepopulateDataToDefault();
            Assert.IsType<OkResult>(repopulate.Result);

            //advance 1 day and verify
            var dayResult = _dayController.AdvanceDays(1);
            Assert.IsType<OkResult>(dayResult.Result);

            //get new results and verify
            var dayResult1 = _invController.GetAllItems();
            Assert.IsType<ActionResult<List<Item>>>(dayResult1.Result);
            var list1 = dayResult1.Result as ActionResult<List<Item>>;
            Assert.IsType<List<Item>>(list1.Value);
            var InventoryItems1 = list1.Value as List<Item>;

            //---advance 1 day
            Assert.Equal(11, InventoryItems1.FirstOrDefault(x=> x.ItemName == "Aged Brie").Quality); //Unique appreciation
            Assert.Equal(4, InventoryItems1.FirstOrDefault(x => x.ItemName == "Cheese").Quality); //Control
            Assert.Equal(48, InventoryItems1.FirstOrDefault(x => x.ItemName == "Giant Slayer").Quality); //Conjured
            Assert.Equal(12, InventoryItems1.FirstOrDefault(x => x.ItemName == "Raging Ogre").Quality); //Concert
            Assert.Equal(80, InventoryItems1.FirstOrDefault(x => x.ItemName == "Hand of Ragnaros").Quality); //Legendary

            //---advance 5 more days, total of 6
            var dayResult2 = _dayController.AdvanceDays(5);
            var InventoryItems2 = _invController.GetAllItems().Result.Value;
            //see if quality values accurate
            Assert.Equal(16, InventoryItems2.FirstOrDefault(x => x.ItemName == "Aged Brie").Quality);
            Assert.Equal(0, InventoryItems2.FirstOrDefault(x => x.ItemName == "Cheese").Quality);
            Assert.Equal(38, InventoryItems2.FirstOrDefault(x => x.ItemName == "Giant Slayer").Quality);
            Assert.Equal(24, InventoryItems2.FirstOrDefault(x => x.ItemName == "Raging Ogre").Quality);
            Assert.Equal(80, InventoryItems2.FirstOrDefault(x => x.ItemName == "Hand of Ragnaros").Quality);

            //---advance 5 more day, total of 11
            var dayResult3 = _dayController.AdvanceDays(5);
            var InventoryItems3 = _invController.GetAllItems().Result.Value;
            //see if quality values accurate
            Assert.Equal(21, InventoryItems3.FirstOrDefault(x => x.ItemName == "Aged Brie").Quality);
            Assert.Equal(0, InventoryItems3.FirstOrDefault(x => x.ItemName == "Cheese").Quality);
            Assert.Equal(28, InventoryItems3.FirstOrDefault(x => x.ItemName == "Giant Slayer").Quality);
            Assert.Equal(0, InventoryItems3.FirstOrDefault(x => x.ItemName == "Raging Ogre").Quality);
            Assert.Equal(80, InventoryItems3.FirstOrDefault(x => x.ItemName == "Hand of Ragnaros").Quality);

            Assert.DoesNotContain(InventoryItems3, x => x.Quality < 0);
        }
        [Fact]
        public void DeleteSingleItemTest()
        {
            var itemToDelete = _service.Items.First(x => true).ItemId;
            var result = _invController.DeleteAnItem(itemToDelete);
            Assert.IsType<OkResult>(result.Result);
            var items = _service.Items.Where(x => true).ToList();
            Assert.Equal(19, items.Count);
        }
        [Fact]
        public void DeleteMultipleItemsTest()
        {
            var itemsToDelete = _service.Items.Where(x=> true).Take(2).Select(n=> n.ItemId).ToArray();
            var result = _invController.DeleteMultipleItems(itemsToDelete);
            Assert.IsType<OkResult>(result.Result);
            var items = _service.Items.Where(x=>true).ToList();
            Assert.Equal(18, items.Count);
        }
        [Fact]
        public void GetAllItemsToTrashTest()
        {
            //TODO
        }
        [Fact]
        public void AddAnItemTest()
        {
            //TODO
        }
        [Fact]
        public void GetAnItemByNameTest()
        {
            //TODO
        }
    }
}
