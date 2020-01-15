using System;

namespace GildedRose.Domain
{
    /// <summary>
    /// Represents a normal inventory item. Quality decreases with each passing day.
    /// </summary>
    public class StandardInventoryItem : InventoryItemBase
    {
        public StandardInventoryItem(Guid id, string name, string category, int initialQuality, int initialSellIn)
            : base(id, name, category, initialQuality, initialSellIn)
        {
        }
    }
}