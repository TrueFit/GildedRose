using GildedRose.Client.Models;
using System.Collections.ObjectModel;

namespace GildedRose.Client.ViewModels
{
    /// <summary>
    /// View model for a category
    /// </summary>
    public class ItemCategoryViewModel : AViewModel<ItemCategoryModel>
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
                NotifyPropertyChanged(nameof(Name));
            }
        }

        private string _name;

        #endregion

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
            Name = category.Name;
            Items = new ObservableCollection<ItemViewModel>();

            foreach (var item in category.Items)
            {
                Items.Add(new ItemViewModel(item));
            }

            Model.PropertyChanged += (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(IItemModel.Name):
                        Name = Model.Name;
                        break;
                }
            };
        }
    }
}
