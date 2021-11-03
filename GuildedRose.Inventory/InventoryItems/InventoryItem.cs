namespace GuildedRose.Inventory.InventoryItems
{
    /// <summary>
    /// Base class for inventory items.
    /// Has protected virtual properties and methods that can be overridden by
    /// subclasses.
    /// </summary>
    public class InventoryItem
    {
        /// <summary>
        /// ID or primary key for database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Category of the item
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Number of days the item must be sold in
        /// </summary>
        public int SellIn { get; set; }

        /// <summary>
        /// The quality of the item which denotes how valuable the item is
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        /// The quality adjustment that should be applied to the item
        /// when the day is incremented.
        /// Must be negative to bring <see cref="Quality"/> down and positive to bring
        /// <see cref="Quality"/> down.
        /// </summary>
        protected virtual int QualityAdjustmentByDay => -1;

        /// <summary>
        /// The quality adjustment factor that should be applied to
        /// <see cref="QualityAdjustmentByDay"/> when the day is incremented
        /// and <see cref="SellIn"/> is less than or equal to 0.
        /// The two values (<see cref="QualityAdjustmentByDay"/> and
        /// <see cref="QualityAdjustmentByDayAfterSellInFactor"/>) are multiplied to
        /// achieve the quality adjustment.
        /// </summary>
        protected virtual int QualityAdjustmentByDayAfterSellInFactor => 2;

        /// <summary>
        /// Minimum quality that the item can have
        /// </summary>
        protected virtual int MinimumQuality => 0;

        /// <summary>
        /// Maximum quality that the item can have
        /// </summary>
        protected virtual int MaximumQuality => 50;

        /// <summary>
        /// Adjustment to be applied to <see cref="SellIn"/> when the day is incremented.
        /// Must be negative to bring <see cref="SellIn"/> down and positive to bring
        /// <see cref="SellIn"/> down.
        /// </summary>
        protected virtual int SellInAdjustmentByDay => -1;

        /// <summary>
        ///  Constructor for <see cref="InventoryItem"/> that applies properties to
        /// corresponding arguments.
        /// </summary>
        /// <param name="name">The name of the item to be applied to <see cref="Name"/></param>
        /// <param name="category">The category of the item to be applied to <see cref="Category"/></param>
        /// <param name="sellIn">Number of days the item must be sold in to be applied to <see cref="SellIn"/></param>
        /// <param name="quality">The quality of the item which denotes how valuable the item is to be applied to <see cref="SellIn"/></param>
        public InventoryItem(string name, string category, int sellIn, int quality)
        {
            Name = name;
            Category = category;
            SellIn = sellIn;
            Quality = quality;
            AdjustQualityBasedOnMinAndMax();
        }

        /// <summary>
        /// Constructor for <see cref="InventoryItem"/>. Takes another <see cref="InventoryItem"/> as
        /// an argument. Clones values for <see cref="Name"/>, <see cref="Category"/>, <see cref="SellIn"/>,
        /// and <see cref="Quality"/>.
        /// Also adjusts <see cref="Quality"/> based on minumum and maximum quality values.
        /// </summary>
        /// <param name="inventoryItem">The <see cref="InventoryItem"/> to clone</param>
        public InventoryItem(InventoryItem inventoryItem)
        {
            Name = inventoryItem.Name;
            Category = inventoryItem.Category;
            SellIn = inventoryItem.SellIn;
            Quality = inventoryItem.Quality;
            AdjustQualityBasedOnMinAndMax();
        }

        /// <summary>
        /// Sets <see cref="Quality"/> to <see cref="MinimumQuality"/> if original
        /// quality is less than <see cref="MinimumQuality"/>.
        /// Sets <see cref="Quality"/> to <see cref="MaximumQuality"/> if original
        /// quality is greater than <see cref="MaximumQuality"/>.
        /// </summary>
        protected virtual void AdjustQualityBasedOnMinAndMax()
        {
            if (Quality < MinimumQuality)
            {
                Quality = MinimumQuality;
            }
            else if (Quality > MaximumQuality)
            {
                Quality = MaximumQuality;
            }
        }

        /// <summary>
        /// Applies changes to properties that are needed when a day passes.
        /// Calls <see cref="UpdateQuality"/> and <see cref="UpdateSellIn"/> in that order
        /// </summary>
        public void IncrementDay()
        {
            UpdateSellIn();
            UpdateQuality();
        }

        /// <summary>
        /// Applies updates for <see cref="Quality"/> based on <see cref="SellIn"/> value
        /// using <see cref="QualityAdjustmentByDay"/>, <see cref="QualityAdjustmentByDayAfterSellInFactor"/>,
        /// <see cref="MinimumQuality"/> and <see cref="MaximumQuality"/>
        /// </summary>
        protected virtual void UpdateQuality()
        {
            if (SellIn <= 0)
            {
                Quality += QualityAdjustmentByDay * QualityAdjustmentByDayAfterSellInFactor;
            }
            else
            {
                Quality += QualityAdjustmentByDay;
            }
            AdjustQualityBasedOnMinAndMax();
        }

        /// <summary>
        /// Applies <see cref="SellInAdjustmentByDay"/> to <see cref="SellIn"/>.
        /// </summary>
        protected virtual void UpdateSellIn()
        {
            SellIn += SellInAdjustmentByDay;
        }
    }
}
