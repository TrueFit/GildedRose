using GildedRose.Domain.Models;
using GildedRose.Domain.Mappers;
using GildedRose.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GildedRose.Domain.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DomainMapperTests
    {

        private IMapper ScaffoldMapper()
        {
            var inventoryMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new InventoryMappingProfile());
            });

            return inventoryMapper.CreateMapper();
        }

        [TestMethod]
        public void DegradationItemMustMap()
        {
            var mapper = ScaffoldMapper();
            var mappedDegradation = mapper.Map<Degradation>(
                new Degradation()
                {
                    DegradationId = 4,
                    Threshold = 0,
                    Interval = 1,
                    Rate = 1,
                    HasNoValuePastExpiration = false
                }

            );

            Assert.AreEqual(4, mappedDegradation.DegradationId);
            Assert.AreEqual(1, mappedDegradation.Interval);
            Assert.AreEqual(0, mappedDegradation.Threshold);
            Assert.AreEqual(1, mappedDegradation.Rate);
            Assert.AreEqual(false, mappedDegradation.HasNoValuePastExpiration);
        }

        [TestMethod]
        public void CategoryItemMustMap()
        {
            var mapper = ScaffoldMapper();
            var mappedCategory = mapper.Map<Category>(
                new Category()
                {
                    CategoryId = 3,
                    Name = "c",
                    MinimumQuality = 0,
                    MaximumQuality = 1,
                    Degradation = new Degradation()
                    {
                        DegradationId = 4,
                        Threshold = 0,
                        Interval = 1,
                        Rate = 1,
                        HasNoValuePastExpiration = false
                    }
                }
            );

            Assert.AreEqual(3, mappedCategory.CategoryId);
            Assert.AreEqual("c", mappedCategory.Name);
            Assert.AreEqual(1, mappedCategory.MaximumQuality);
            Assert.AreEqual(0, mappedCategory.MinimumQuality);

            Assert.AreEqual(4, mappedCategory.Degradation.DegradationId);
            Assert.AreEqual(1, mappedCategory.Degradation.Interval);
            Assert.AreEqual(0, mappedCategory.Degradation.Threshold);
            Assert.AreEqual(1, mappedCategory.Degradation.Rate);
            Assert.AreEqual(false, mappedCategory.Degradation.HasNoValuePastExpiration);
        }

        [TestMethod]
        public void InventoryQualityMustMap()
        {
            var mapper = ScaffoldMapper();
            var mappedQuality = mapper.Map<QualityValue>(
                new Quality()
                {
                    QualityId = 5,
                    Initial = 40,
                    Current = 30
                }
            );

            Assert.AreEqual(5, mappedQuality.QualityId);
            Assert.AreEqual(40, mappedQuality.Initial);
            Assert.AreEqual(30, mappedQuality.Current);

        }

        [TestMethod]
        public void InventoryItemMustMap()
        {
            var mapper = ScaffoldMapper();
            var mappedItem = mapper.Map<InventoryItemValue>(
                new InventoryItem()
                {
                    InventoryId = 1,
                    InventoryItemId = 2,
                    Name = "a",
                    PurchasedOn = DateTime.Today,
                    SellIn = 100,
                    Category = new Category()
                    {
                        CategoryId = 3,
                        Name = "c",
                        MinimumQuality = 0,
                        MaximumQuality = 1,
                        Degradation = new Degradation()
                        {
                            DegradationId = 4,
                            Threshold = 0,
                            Interval = 1,
                            Rate = 1,
                            HasNoValuePastExpiration = false
                        }
                    },
                    Quality = new Quality()
                    {
                        QualityId = 5,
                        Initial = 40,
                        Current = 30
                    }
                });

            Assert.AreEqual(1, mappedItem.InventoryId);
            Assert.AreEqual(2, mappedItem.InventoryItemId);
            Assert.AreEqual("a", mappedItem.Name);
            Assert.AreEqual(100, mappedItem.SellIn);
            Assert.AreEqual(DateTime.Today, mappedItem.PurchasedOn);

            Assert.AreEqual(5, mappedItem.Quality.QualityId);
            Assert.AreEqual(40, mappedItem.Quality.Initial);
            Assert.AreEqual(30, mappedItem.Quality.Current);

            Assert.AreEqual(3, mappedItem.Category.CategoryId);
            Assert.AreEqual("c", mappedItem.Category.Name);
            Assert.AreEqual(1, mappedItem.Category.MaximumQuality);
            Assert.AreEqual(0, mappedItem.Category.MinimumQuality);

            Assert.AreEqual(4, mappedItem.Category.Degradation.DegradationId);
            Assert.AreEqual(1, mappedItem.Category.Degradation.Interval);
            Assert.AreEqual(0, mappedItem.Category.Degradation.Threshold);
            Assert.AreEqual(1, mappedItem.Category.Degradation.Rate);
            Assert.AreEqual(false, mappedItem.Category.Degradation.HasNoValuePastExpiration);

        }

        [TestMethod]
        public void InventoryMustMap()
        {
            var mapper = ScaffoldMapper();
            var mappedInventory = mapper.Map<Inventory>(
                new InventoryValue()
                {
                    InventoryId = 1,
                    Name = "a",
                    Owner = "o",
                    CurrentDate = DateTime.Today
                });

            Assert.AreEqual(1, mappedInventory.InventoryId);
            Assert.AreEqual("a", mappedInventory.Name);
            Assert.AreEqual("o", mappedInventory.Owner);
            Assert.AreEqual(DateTime.Today, mappedInventory.CurrentDate);
        }
    }
}
