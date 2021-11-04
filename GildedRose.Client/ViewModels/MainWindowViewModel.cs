using GildedRose.Client.InventorySystems;
using GildedRose.Client.Models;
using GildedRose.Client.Views;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            _inventorySystem = new GrpcInventorySystem();

            ItemCategories = new ObservableCollection<ItemCategoryViewModel>();

            GoToSleepCommand = new SimpleCommand(() =>
            {
                _inventorySystem.ProgressToNextDay();
                RefreshInventory();
            });

            ThrowAwayTrashCommand = new SimpleCommand(() =>
            {
                _inventorySystem.RemoveTrash();
                RefreshInventory();
            });

            AddNewItemCommand = new SimpleCommand(() =>
            {
                var viewModel = new AddItemWindowViewModel();
                var dialog = new AddItemWindowView(viewModel);

                var result = dialog.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    _inventorySystem.AddItem(new ItemModel()
                    {
                        Name = viewModel.Name,
                        Category = viewModel.Category,
                        SellIn = viewModel.SellIn,
                        Quality = viewModel.Quality
                    });

                    RefreshInventory();
                }
            });
        }

        /// <summary>
        /// Initialize the client by connecting it to the inventory system.
        /// </summary>
        public void Initialize()
        {
            _inventorySystem.Connect();

            RefreshInventory();
        }

        private void RefreshInventory()
        {
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
        }
    }
}
