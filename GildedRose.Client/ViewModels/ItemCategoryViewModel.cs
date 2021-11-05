using GildedRose.Client.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace GildedRose.Client.ViewModels
{
    /// <summary>
    /// View model for an item category
    /// </summary>
    public class ItemCategoryViewModel : AViewModel<ItemCategoryModel>, IDisposable
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
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            Name = category.Name;
            Items = new ObservableCollection<ItemViewModel>();

            foreach (var item in category.Items)
            {
                Items.Add(new ItemViewModel(item));
            }

            // Make sure we got notified about model changes.
            Model.Items.CollectionChanged += Items_CollectionChanged;

            Model.PropertyChanged += Model_PropertyChanged;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (Model != null)
            {
                if (Model.Items != null)
                    Model.Items.CollectionChanged -= Items_CollectionChanged;

                Model.PropertyChanged -= Model_PropertyChanged;
            }
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Add new items to the category.
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newObject in e.NewItems)
                {
                    if (newObject is IItemModel newItem)
                        Items.Add(new ItemViewModel(newItem));
                }
            }
            // Remove items from the category.
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldObject in e.OldItems)
                {
                    if (oldObject is IItemModel oldItem)
                    {
                        var item = Items.FirstOrDefault(x => x.Model.Id == oldItem.Id);
                        if (item != null)
                        {
                            Items.Remove(item);

                            item.Dispose();
                        }
                    }
                }
            }
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IItemModel.Name):
                    Name = Model.Name;
                    break;
            }
        }
    }
}
