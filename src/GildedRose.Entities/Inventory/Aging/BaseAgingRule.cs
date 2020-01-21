namespace GildedRose.Entities.Inventory.Aging
{
    /// <summary>
    /// Base class for aging rules
    /// </summary>
    public abstract class BaseAgingRule
    {
        /// <summary>
        /// Name of the rule
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the item this rule applies to (if applicable)
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Name of the item category this rule applies to (if applicable)
        /// </summary>
        public string ItemCategory { get; set; }

        public int MaxQuality { get; set; }

        /// <summary>
        /// Applying aging logic to adjust the SellIn and Quality of the <see cref="Inventory" /> item.
        /// </summary>
        /// <param name="item"></param>
        public void ApplyAging(Inventory item)
        {
            // First we set the SellIn
            SetSellIn(item);

            // Next we set the Quality (using the updated SellIn value)
            SetQuality(item);

            // If quality has gone to zero, change sell in to zero
            if (item.Quality == 0 && item.SellIn > 0)
            {
                item.SellIn = 0;
            }
        }

        protected abstract void SetQuality(Inventory item);

        protected virtual void SetSellIn(Inventory item)
        {
            if (item.SellIn > 0)
            {
                item.SellIn -= 1;
            }
        }
    }
}
