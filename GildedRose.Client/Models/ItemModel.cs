namespace GildedRose.Client.Models
{
    /// <summary>
    /// An item that is available for purchase in the Gilded Rose.
    /// </summary>
    public class ItemModel : IItemModel
    {
        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public string Category { get; set; }

        /// <inheritdoc />
        public int SellIn { get; set; }

        /// <inheritdoc />
        public int Quality { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Name = \"{Name}\", Category = \"{Category}\", SellIn = {SellIn}, Quality = {Quality}";
        }
    }
}
