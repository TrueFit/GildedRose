using System;

namespace GildedRose.Domain
{
    public abstract class InventoryItemBase : IInventoryItem
    {
        private readonly string category;
        private readonly Guid id;
        private readonly int maxQuality;
        private readonly string name;
        private int quality;
        private int? sellIn;

        protected InventoryItemBase(Guid id, string name, string category, int initialQuality, int? initialSellIn = null, int maxQuality = 50)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Missing name for inventory item", nameof(name));
            }

            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentException("Missing category for inventory item", nameof(category));
            }

            if (maxQuality < 0)
            {
                throw new ArgumentException("maxQuality must be non-negative", nameof(maxQuality));
            }

            if (initialQuality < 0 || initialQuality > maxQuality)
            {
                throw new ArgumentException($"The initial quality of this inventory item must be between 0 and {maxQuality}.", nameof(initialQuality));
            }

            this.category = category;
            this.id = id;
            this.maxQuality = maxQuality;
            this.name = name;
            this.quality = initialQuality;
            this.sellIn = initialSellIn;
        }

        public string Category => this.category;

        public Guid Id => this.id;

        public string Name => this.name;

        public int Quality
        {
            get => this.quality;

            protected set
            {
                this.quality = value < 0 ? 0 : value > this.maxQuality ? this.maxQuality : value;
            }
        }

        public int? SellIn => this.sellIn;

        public void OnAdvanceToNextDay()
        {
            if (this.SellIn.HasValue)
            {
                this.sellIn--;
            }

            this.AdjustQuality();
        }

        protected virtual void AdjustQuality()
        {
            if (this.SellIn == null || this.SellIn > 0)
            {
                this.Quality -= 1;
            }
            else
            {
                this.Quality -= 2;
            }
        }
    }
}