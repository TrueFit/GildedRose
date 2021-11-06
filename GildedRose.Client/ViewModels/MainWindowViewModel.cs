using GildedRose.Client.InventorySystems;
using GildedRose.Client.Models;
using GildedRose.Client.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace GildedRose.Client.ViewModels
{
    /// <summary>
    /// View model for the application's main window
    /// </summary>
    public class MainWindowViewModel : AViewModel
    {
        private readonly IInventorySystem _inventorySystem;

        /// <summary>
        /// The inventory categories
        /// </summary>
        public ObservableCollection<ItemCategoryViewModel> ItemCategories { get; }

        /// <summary>
        /// Command to end the work day
        /// </summary>
        public ICommand GoToSleepCommand { get; }

        /// <summary>
        /// Command to add a new item
        /// </summary>
        public ICommand AddNewItemCommand { get; }

        /// <summary>
        /// Command to trow away any trash
        /// </summary>
        public ICommand ThrowAwayTrashCommand { get; }

        #region TotalWorth

        /// <summary>
        /// The total worth of the inventory.
        /// </summary>
        public int TotalWorth
        {
            get { return _totalWorth; }
            set
            {
                _totalWorth = value;
                NotifyPropertyChanged(nameof(TotalWorth));
            }
        }

        private int _totalWorth;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            _inventorySystem = new GrpcInventorySystem();

            ItemCategories = new ObservableCollection<ItemCategoryViewModel>();

            GoToSleepCommand = new SimpleCommand(() =>
            {
                var progressedItems = _inventorySystem.ProgressToNextDay();

                // Update the quality and sell-in value for each item.
                foreach (var progressedItem in progressedItems)
                {
                    var item = GetItem(progressedItem.Id);
                    if (item != null)
                    {
                        item.SellIn = progressedItem.SellIn;
                        item.Quality = progressedItem.Quality;
                    }
                }

                // Update inventory's worth.
                TotalWorth = _inventorySystem.GetTotalWorth();
            });

            ThrowAwayTrashCommand = new SimpleCommand(() =>
            {
                var removedItems = _inventorySystem.RemoveTrash();

                // Remove all the trashed items from the inventory.
                foreach (var removedItem in removedItems)
                {
                    var category = GetCategory(removedItem);
                    if (category != null)
                    {
                        var item = category.Model.Items.FirstOrDefault(x => x.Id == removedItem);
                        if (item != null)
                            category.Model.Items.Remove(item);

                        // If the category does not contain any items any more, we will delete it.
                        if (category.Model.Items.Count == 0)
                        {
                            ItemCategories.Remove(category);

                            category.Dispose();
                        }
                    }
                }

                // Update inventory's worth.
                TotalWorth = _inventorySystem.GetTotalWorth();
            });

            AddNewItemCommand = new SimpleCommand(() =>
            {
                var viewModel = new AddItemWindowViewModel();
                var dialog = new AddItemWindowView(viewModel);

                var result = dialog.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    // Create new item ..
                    var newItem = new ItemModel()
                    {
                        Id = Guid.NewGuid(),
                        Name = viewModel.Name,
                        Category = viewModel.Category,
                        SellIn = viewModel.SellIn,
                        Quality = viewModel.Quality
                    };

                    // .. and let the inventory system know about it.
                    _inventorySystem.AddItem(newItem);

                    // Update models and user interface.
                    var category = ItemCategories.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) && x.Name.Equals(newItem.Category));
                    if (category == null)
                    {
                        category = new ItemCategoryViewModel(new ItemCategoryModel()
                        {
                            Name = newItem.Category
                        });
                        ItemCategories.Add(category);
                    }

                    category.Items.Add(new ItemViewModel(newItem));
                }

                // Update inventory's worth.
                TotalWorth = _inventorySystem.GetTotalWorth();
            });
        }

        /// <summary>
        /// Initialize the client by connecting it to the inventory system.
        /// </summary>
        public void Initialize()
        {
            _inventorySystem.Connect();

            // Get information about all our items.
            var allItems = _inventorySystem.GetAllItems();

            // Clear view model.
            ItemCategories.Clear();

            // Sort items into categories.
            var categories = new List<ItemCategoryModel>();
            foreach (var item in allItems)
            {
                var category = categories.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) && x.Name.Equals(item.Category));
                if (category == null)
                {
                    category = new ItemCategoryModel()
                    {
                        Name = item.Category
                    };
                    categories.Add(category);
                }

                category.Items.Add(item);
            }

            // Add categories to view model.
            foreach (var category in categories)
                ItemCategories.Add(new ItemCategoryViewModel(category));

            // Set inventory's worth.
            TotalWorth = _inventorySystem.GetTotalWorth();
        }

        private ItemCategoryViewModel GetCategory(Guid id)
        {
            return ItemCategories.FirstOrDefault(x => x.Items.Any(y => y.Model.Id == id));
        }

        private IItemModel GetItem(Guid id)
        {
            var category = GetCategory(id);
            if (category != null)
            {
                return category.Items.FirstOrDefault(x => x.Model.Id == id).Model;
            }

            return null;
        }
    }
}
