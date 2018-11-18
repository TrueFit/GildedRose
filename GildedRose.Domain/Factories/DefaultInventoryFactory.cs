using AutoMapper;
using GildedRose.Domain.Models;
using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GildedRose.Domain.Factories
{
    public class DefaultInventoryFactory : IDataFactory
    {
        private readonly IDatabase<Inventory> _inventories;
        private readonly IMapper _mapper;
        private readonly InventoryContext _context;
        private readonly InventoryValue _inventoryValue;
        private Dictionary<int, string> _categories;
        private int _inventoryId;

        public DefaultInventoryFactory(InventoryValue inventoryValue, IDatabase<Inventory> inventories, IMapper mapper, InventoryContext context)
        {
            if (inventoryValue == null)
            {
                throw new ArgumentNullException(nameof(inventoryValue));
            }
            _mapper = mapper;
            _inventories = inventories;
            _context = context;
            _inventoryValue = inventoryValue;
        }

        public void Scaffold()
        {
            AddInventory();
            AddDefaultCategories();
            AddDefaultItems();
        }

        private void AddInventory()
        {
            var inventoryData = new InventoryData(_context);
            var inventoryDomain = new InventoryDomain(inventoryData, _mapper, _context);

            inventoryDomain.AddInventory(_inventoryValue);
            var addedInventory = inventoryDomain.GetLastAddedInventory();
            _inventoryId = addedInventory.InventoryId;
        }

        private void AddDefaultCategories()
        {
#warning Normally, I would not new up domains in another domain I'd publish a message to perform this work in other domains to keep things loosely coupled.  Given time contraints, I'm accessing the other domains directly.
            var categoryData = new CategoryData(_context);
            var categoryDomain = new CategoryDomain(categoryData, _mapper);

            var categories = BuildDefaultCategories();
            foreach (var category in categories)
            {
                categoryDomain.AddCategory(category);
            }

            _categories = categoryDomain.GetCategories().ToDictionary(c => c.CategoryId, c => c.Name);
        }

        private void AddDefaultItems()
        {
#warning Normally, I would not new up domains in another domain I'd publish a message to perform this work in other domains to keep things loosely coupled.  Given time contraints, I'm accessing the other domains directly.
            var itemData = new ItemData(_context);
            var itemDomain = new InventoryItemDomain(itemData, _mapper);

            var items = BuildDefaultInventoryItems();
            foreach (var item in items)
            {
                itemDomain.AddInventoryItem(item);
            }
        }

        private List<CategoryValue> BuildDefaultCategories()
        {
            var categories = new List<CategoryValue>();

            categories.Add(BuildStandardCategory("Weapon"));
            categories.Add(BuildStandardCategory("Armor"));
            categories.Add(BuildStandardCategory("Food"));
            categories.Add(BuildStandardCategory("Potion"));
            categories.Add(BuildStandardCategory("Misc"));

            var sulfurasCategory = BuildStandardCategory("Sulfuras");
            sulfurasCategory.MaximumQuality = 80;
            sulfurasCategory.MinimumQuality = 80;
            categories.Add(sulfurasCategory);

            var backstagePassCategory = BuildStandardCategory("Backstage Passes");
            backstagePassCategory.Degradation.Interval = 5;
            backstagePassCategory.Degradation.Threshold = 10;
            backstagePassCategory.Degradation.Rate = -1;
            backstagePassCategory.Degradation.HasNoValuePastExpiration = true;
            categories.Add(backstagePassCategory);

            var agedFoodCategory = BuildStandardCategory("Aged Food");
            agedFoodCategory.Degradation.Rate = -1;
            categories.Add(agedFoodCategory);

            var conjuredCategory = BuildStandardCategory("Conjured");
            agedFoodCategory.Degradation.Rate = 2;
            categories.Add(conjuredCategory);

            return categories;
        }

        private CategoryValue BuildStandardCategory(string name)
        {
            return new CategoryValue()
            {
                Name = name,
                Degradation = new DegradationValue()
            };
        }

        private List<InventoryItemValue> BuildDefaultInventoryItems()
        {
            var items = new List<InventoryItemValue>();

            items.Add(BuildItem("Sword", "Weapon", 30, 50, _inventoryId));
            items.Add(BuildItem("Axe", "Weapon", 40, 50, _inventoryId));
            items.Add(BuildItem("Halberd", "Weapon", 60, 40, _inventoryId));
            items.Add(BuildItem("Aged Brie", "Aged Food", 50, 10, _inventoryId));
            items.Add(BuildItem("Aged Milk", "Food", 20, 20, _inventoryId));
            items.Add(BuildItem("Mutton", "Food", 10, 10, _inventoryId));
            items.Add(BuildItem("Hand of Ragnaros", "Sulfuras", 80, 80, _inventoryId));
            items.Add(BuildItem("I am Murlock", "Backstage Passes", 20, 10, _inventoryId));
            items.Add(BuildItem("Ragin Ogre", "Backstage Passes", 10, 10, _inventoryId));
            items.Add(BuildItem("Giant Slayer", "Conjured", 15, 50, _inventoryId));
            items.Add(BuildItem("Storm Hammer", "Conjured", 20, 50, _inventoryId));
            items.Add(BuildItem("Belt of Giant Strength", "Conjured", 20, 40, _inventoryId));
            items.Add(BuildItem("Cheese", "Food", 5, 5, _inventoryId));
            items.Add(BuildItem("Potion of Healing", "Potion", 10, 10, _inventoryId));
            items.Add(BuildItem("Bag of Holding", "Misc", 10, 50, _inventoryId));
            items.Add(BuildItem("TAFKAL80ETC Concert", "Backstage Passes", 15, 20, _inventoryId));
            items.Add(BuildItem("Elixer of the Mongoose", "Potion", 5, 7, _inventoryId));
            items.Add(BuildItem("+5 Dexterity Vest", "Armor", 10, 20, _inventoryId));
            items.Add(BuildItem("Full Plate Mail", "Armor", 50, 50, _inventoryId));
            items.Add(BuildItem("Wooden Sheild", "Armor", 10, 30, _inventoryId));

            return items;
        }

        private InventoryItemValue BuildItem(string name, string category, int sellIn, int quality, int inventoryId)
        {
            var categoryKey = _categories.First(c => c.Value == category).Key;

            return new InventoryItemValue()
            {
                InventoryId = inventoryId,
                Name = name,
                PurchasedOn = DateTime.Today,
                SellIn = sellIn,
                Category = new CategoryValue()
                {
                    CategoryId = categoryKey
                },
                Quality = new QualityValue()
                {
                    Initial = quality,
                    Current = quality
                }
            };
        }

    }
}
