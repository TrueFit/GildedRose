namespace GildedRose.Entities.Inventory.Aging
{
    /// <summary>
    /// Base quality modifier class for aging rules
    /// </summary>
    public class BaseQualityModifier
    {
        /// <summary>
        /// Represent the amount to adjust the quality of an inventory item when aged by one day
        /// </summary>
        public int Modifier { get; set; }
    }
}
