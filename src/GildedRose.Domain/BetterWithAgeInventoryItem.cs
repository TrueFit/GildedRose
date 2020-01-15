using System;

namespace GildedRose.Domain
{
    /// <summary>
    /// Represents an inventory item that increases in quality with each passing day.
    /// </summary>
    public class BetterWithAgeInventoryItem : InventoryItemBase
    {
        public BetterWithAgeInventoryItem(Guid id, string name, string category, int initialQuality)
            : base(id, name, category, initialQuality)
        {
        }

        protected override void AdjustQuality()
        {
            this.Quality += 1;
        }
    }
}