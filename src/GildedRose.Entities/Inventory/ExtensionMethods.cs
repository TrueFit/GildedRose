namespace GildedRose.Entities.Inventory
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// This extension provides a convenient way to create an <see cref="InventoryHistory" /> instance
        /// from an <see cref="Inventory" /> instance 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static InventoryHistory ToInventoryHistory(this Inventory item)
        {
            return new InventoryHistory
            {
                Id = item.Id,
                Name = item.Name,
                Category = item.Category,
                SellIn = item.SellIn,
                Quality = item.Quality,
                InventoryAddedDate = item.InventoryAddedDate
            };
        }
    }
}
