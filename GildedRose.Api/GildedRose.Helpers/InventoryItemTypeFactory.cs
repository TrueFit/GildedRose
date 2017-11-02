using System;
using System.Collections.Generic;
using System.Reflection;
using GildedRose.BusinessObjects;
using GildedRose.Helpers.ItemTypes;

namespace GildedRose.Helpers
{
    public class InventoryItemTypeFactory
    {
        private Dictionary<string, Type> _itemTypes;

        public InventoryItemTypeFactory()
        {
            LoadReturnTypes();
        }

        public IItemType CreateInstance(InventoryItem i)
        {
            var t = GetTypeToCreate(i.Category, i.Name);

            if (t == null) return new DefaultType();

            return Activator.CreateInstance(t) as IItemType;
        }

        private Type GetTypeToCreate(string category, string name)
        {
            foreach (var itemType in _itemTypes)
            {
                // replace spaces so we can access the class name
                if (itemType.Key.Contains(category.Replace(" ", "")))
                {
                    return _itemTypes[itemType.Key];
                }
                // special case for Aged Brie
                if (itemType.Key.Contains(name.Replace(" ", "")))
                {
                    return _itemTypes[itemType.Key];
                }
            }
            return null;
        }

        void LoadReturnTypes()
        {
            _itemTypes = new Dictionary<string, Type>();
            Type[] typesInAssembly = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var type in typesInAssembly)
            {
                if (type.GetInterface(typeof(IItemType).ToString()) != null)
                {
                    _itemTypes.Add(type.Name, type);
                }
            }
        }
    }
}
