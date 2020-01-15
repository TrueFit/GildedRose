using System;

namespace GildedRose.Domain
{
    /// <summary>
    /// Represents an inventory item whose quality decreases at double the rate of a normal item.
    /// </summary>
    public class ConjuredInventoryItem : InventoryItemBase
    {
        public ConjuredInventoryItem(Guid id, string name, string category, int initialQuality, int initialSellIn)
            : base(id, name, category, initialQuality, initialSellIn)
        {
        }

        protected override void AdjustQuality()
        {
            if (this.SellIn.Value >= 0)
            {
                this.Quality -= 2;
            }
            else
            {
                this.Quality -= 4;
            }
        }
    }
}