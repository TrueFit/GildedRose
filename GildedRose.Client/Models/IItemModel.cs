namespace GildedRose.Client.Models
{
    /// <summary>
    /// An interface for an item that is available for purchase in the Gilded Rose.
    /// </summary>
    public interface IItemModel : IModel
    {
        /// <summary>
        /// An item's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// An item's category.
        /// </summary>
        string Category { get; }

        /// <summary>
        /// The SellIn value denotes the number of days the store owners have to sell the item.
        /// </summary>
        int SellIn { get; }

        /// <summary>
        /// The quality of an item states how valuable the item is.
        /// </summary>
        int Quality { get; }
    }
}
