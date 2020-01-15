using System;

namespace GildedRose.Domain
{
    /// <summary>
    /// Represents an inventory item with a fixed quality.
    /// </summary>
    public class LegendaryInventoryItem : InventoryItemBase
    {
        public LegendaryInventoryItem(Guid id, string name, string category, int quality)
            : base(id, name, category, quality, maxQuality: quality)
        {
        }

        protected override void AdjustQuality()
        {
        }
    }
}