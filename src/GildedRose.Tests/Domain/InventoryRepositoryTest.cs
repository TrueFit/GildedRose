#region Using Directives

using System;
using System.Linq;
using GildedRose.Data;
using GildedRose.Domain.Inventory;
using GildedRose.Entities.Inventory;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

#endregion

namespace GildedRose.Tests.Domain
{
    /// <summary>
    /// This test is executing as an integration test
    /// I wouldn't typically setup a test like this as an integration test but it was a time saver and help verify
    /// the Entity Framework provider works as expected (first time using it)
    /// </summary>
    public class InventoryRepositoryTest : IClassFixture<BaseTestFixture>
    {
        private BaseTestFixture _fixture;

        public InventoryRepositoryTest(BaseTestFixture fixture)
        {
            _fixture = fixture;
            DataContextHelper.SetupCleanDatabase();
        }

        [Fact]
        public void GetList_ReturnsAllInventory()
        {
            using (var scope = _fixture.ServiceProvider.CreateScope())
            {
                var repo = (IInventoryRepository)scope.ServiceProvider.GetService(typeof(IInventoryRepository));

                var taskResult = repo.GetList();
                var inventory = taskResult.Result.ToList();

                Assert.Equal(20, inventory.Count);
            }
        }

        [Theory,
         InlineData("w", new[] {"Sword", "Wooden Shield"}), // this is a partial match with multiple hits (1 lowercase and 1 uppercase match) 
         InlineData("Storm Hammer", new[] {"Storm Hammer"}) // this is an exact match
        ]
        public void GetByName_WithNameFilter_ReturnsMatchingItems(string name, string[] expectedNames)
        {
            using (var scope = _fixture.ServiceProvider.CreateScope())
            {
                var repo = (IInventoryRepository)scope.ServiceProvider.GetService(typeof(IInventoryRepository));

                var taskResult = repo.GetByName(name);
                var inventory = taskResult.Result.ToList();

                Assert.Equal(expectedNames.Length, inventory.Count);
                for (var i = 0; i < expectedNames.Length; i++)
                {
                    Assert.Equal(expectedNames[i], inventory[i].Name);
                }
            }
        }

        [Fact]
        public void GetHistory_ReturnsHistory()
        {
            using (var scope = _fixture.ServiceProvider.CreateScope())
            {
                var repo = (IInventoryRepository)scope.ServiceProvider.GetService(typeof(IInventoryRepository));

                var history = repo.GetHistory().Result.ToList();
                var inventory = repo.GetList().Result.ToList();

                Assert.Equal(inventory.Count, history.Count); // the freshly loaded DB should have matching inventory and history

                // We are using a working assumption of a static set of data so let's assert that all entity properties have valid values
                var firstItem = history[0];
                Assert.Equal("+5 Dexterity Vest", firstItem.Name);
                Assert.Equal("Armor", firstItem.Category);
                Assert.Equal(10, firstItem.SellIn);
                Assert.Equal(20, firstItem.Quality);
                Assert.True(firstItem.InventoryAddedDate > DateTime.MinValue); // Ensure we don't have the default DateTime value
                Assert.True(firstItem.LastModifiedDate > DateTime.MinValue); // Ensure we don't have the default DateTime value
            }
        }

        [Fact]
        public void GetTrash_HavingItemsWithQualityOfZero_ReturnsTrash()
        {
            using (var scope = _fixture.ServiceProvider.CreateScope())
            {
                var repo = (IInventoryRepository)scope.ServiceProvider.GetService(typeof(IInventoryRepository));

                var taskResult = repo.GetList();
                var allInventory = taskResult.Result.ToList();

                var trashCount = allInventory.Count(i => i.Quality == 0);
                Assert.Equal(0, trashCount);

                var inventory = allInventory.Where(i => i.Category == "Backstage Passes");
                var counter = 0;
                foreach (var item in inventory)
                {
                    item.Quality = 0;
                    counter++;
                }
                repo.Update(inventory, InventoryHistoryAction.Modified);

                var trash = repo.GetTrash().Result.ToList();

                Assert.Equal(counter, trash.Count);
            }
        }

        [Fact]
        public void PerformAging_WithInventory_AgesInventoryAndUpdatesHistory()
        {
            using (var scope = _fixture.ServiceProvider.CreateScope())
            {
                var repo = (IInventoryRepository)scope.ServiceProvider.GetService(typeof(IInventoryRepository));

                var taskResult = repo.GetList();
                var inventory = taskResult.Result.ToList();
                
                var origItem = inventory.First().ToInventoryHistory();

                var result = repo.PerformAging().Result;

                var updatedItem = repo.GetList().Result.First(i => i.Id == origItem.Id);

                Assert.Equal(origItem.SellIn - 1, updatedItem.SellIn);
                Assert.Equal(origItem.Quality - 1, updatedItem.Quality);
            }
        }
    }
}
