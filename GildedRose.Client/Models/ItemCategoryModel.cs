using System.Collections.Generic;

namespace GildedRose.Client.Models
{
    /// <summary>
    /// A category in Gilded Rose's inventory
    /// </summary>
    public class ItemCategoryModel : IModel
    {
        /// <summary>
        /// The category's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The items in this category
        /// </summary>
        public List<IItemModel> Items { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ItemCategoryModel()
        {
            Items = new List<IItemModel>();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Name = \"{Name}\", {Items.Count} items";
        }
    }
}
