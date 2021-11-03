using GildedRose.Client.Models;

namespace GildedRose.Client.ViewModels
{
    /// <summary>
    /// View model for an item
    /// </summary>
    public class ItemViewModel : AViewModel<IItemModel>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item">The item</param>
        public ItemViewModel(IItemModel item)
            : base(item)
        {
        }
    }
}
