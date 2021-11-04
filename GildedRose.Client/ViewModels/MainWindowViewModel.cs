using GildedRose.Client.Logic;
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
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            _itemUpdateLogic = new ItemUpdateLogic();

            ItemCategories = new ObservableCollection<ItemCategoryViewModel>();

            GoToSleepCommand = new SimpleCommand(() => _itemUpdateLogic.UpdateItems(ItemCategories));
        }
    }
}
