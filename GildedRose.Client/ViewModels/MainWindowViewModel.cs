using GildedRose.Client.Logic;
using GildedRose.Client.Models;
using GildedRose.Client.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GildedRose.Client.ViewModels
{
    /// <summary>
    /// View model for the application's main window
    /// </summary>
    public class MainWindowViewModel : AViewModel
    {
        private readonly ItemUpdateLogic _itemUpdateLogic;

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
            _itemUpdateLogic = new ItemUpdateLogic();

            ItemCategories = new ObservableCollection<ItemCategoryViewModel>();

            GoToSleepCommand = new SimpleCommand(() => _itemUpdateLogic.UpdateItems(ItemCategories));
            ThrowAwayTrashCommand = new SimpleCommand(() => _itemUpdateLogic.RemoveTrash(ItemCategories));

            AddNewItemCommand = new SimpleCommand(() =>
            {
                var viewModel = new AddItemWindowViewModel();
                var dialog = new AddItemWindowView(viewModel);

                var result = dialog.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    _itemUpdateLogic.AddItem(ItemCategories, new ItemModel()
                    {
                        Name = viewModel.Name,
                        Category = viewModel.Category,
                        SellIn = viewModel.SellIn,
                        Quality = viewModel.Quality
                    });
                }
            });
        }
    }
}
