using System.Collections.ObjectModel;

namespace GildedRose.Client.Models
{
    /// <summary>
    /// A category in Gilded Rose's inventory
    /// </summary>
    public class ItemCategoryModel : AModel, IModel
    {
        #region Name

        /// <summary>
        /// The category's name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _name;

        #endregion

        /// <summary>
        /// The items in this category
        /// </summary>
        public ObservableCollection<IItemModel> Items { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ItemCategoryModel()
        {
            Items = new ObservableCollection<IItemModel>();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Name = \"{Name}\", {Items.Count} items";
        }
    }
}
