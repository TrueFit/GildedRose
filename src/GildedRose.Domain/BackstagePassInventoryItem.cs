using System;

namespace GildedRose.Domain
{
    /// <summary>
    /// Represents an inventory item for a backstage pass. This item increases in quality as time
    /// progresses, with greater increases as the expiration date approaches, but falls to 0 after
    /// the expiration date.
    /// </summary>
    public class BackstagePassInventoryItem : InventoryItemBase
    {
        public BackstagePassInventoryItem(Guid id, string name, string category, int initialQuality, int initialSellIn)
            : base(id, name, category, initialQuality, initialSellIn)
        {
        }

        protected override void AdjustQuality()
        {
            if (this.SellIn <= 0)
            {
                this.Quality = 0;
            }
            else if (this.SellIn <= 5)
            {
                this.Quality += 3;
            }
            else if (this.SellIn <= 10)
            {
                this.Quality += 2;
            }
            else
            {
                this.Quality += 1;
            }
        }
    }
}