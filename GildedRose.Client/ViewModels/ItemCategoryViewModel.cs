using GildedRose.Client.Models;
using System.Collections.ObjectModel;

namespace GildedRose.Client.ViewModels
{
    /// <summary>
    /// View model for a category
    /// </summary>
    public class ItemCategoryViewModel : AViewModel<ItemCategoryModel>
    {
        /// <summary>
        /// The items in this category
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="category">The category</param>
        public ItemCategoryViewModel(ItemCategoryModel category)
            : base(category)
        {
            Items = new ObservableCollection<ItemViewModel>();

            foreach (var item in category.Items)
            {
                Items.Add(new ItemViewModel(item));
            }
        }
    }
}
