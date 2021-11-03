namespace GuildedRose.Inventory.InventoryItems
{
    /// <summary>
    /// Represents an <see cref="InventoryItem"/> with special requirements for the "Backstage Passes" category.
    /// Adjusts quality adjustment by day to increase by 2 when sell in is less than or equal to 10.
    /// Adjusts quality adjustment by day to increase by 3 when sell in is less than or equal to 5.
    /// Adjusts quality adjustment by day to increase by the negative of the default for <see cref="InventoryItem"/>
    /// when <see cref="InventoryItem.SellIn"/> is greater than 10
    /// Adjusts quality adjustment by day to decrease by the negative of the current quality
    /// when <see cref="InventoryItem.SellIn"/> is less than or equal to 0
    /// </summary>
    public class BackStagePassesInventoryItem : InventoryItem
    {
        protected override int QualityAdjustmentByDay
        {
            get
            {
                if (SellIn <= 0)
                {
                    return Quality * -1;
                }
                else if (SellIn <= 5)
                {
                    return 3;
                }
                else if (SellIn <= 10)
                {
                    return 2;
                }
                else
                {
                    return base.QualityAdjustmentByDay * -1;
                }
            }   
        }

        public BackStagePassesInventoryItem(InventoryItem inventoryItem) : base(inventoryItem)
        {
        }
    }
}
