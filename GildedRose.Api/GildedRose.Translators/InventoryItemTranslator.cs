using GildedRose.BusinessObjects;
using GildedRose.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GildedRose.Translators
{
    public static class InventoryItemTranslator
    {
        public static IEnumerable<InventoryItem> AsInventoryItems(
            this IEnumerable<GildedRoseInventoryItem> values)
        {
            return values.Select(x => x.AsInventoryItem()).ToList();
        }

        public static InventoryItem AsInventoryItem(this GildedRoseInventoryItem value)
        {
            return (value == null)
                ? null
                : new InventoryItem()
                {
                    Id = value.id,
                    Name = value.name,
                    Category = value.category,
                    Sellin = value.sellin,
                    Quality = value.quality
                };
        }
    }
}
