using System.Collections.ObjectModel;

namespace GildedRose.Client.ViewModels
{
    /// <summary>
    /// View model for the application's main window
    /// </summary>
    public class MainWindowViewModel : AViewModel
    {
        /// <summary>
        /// The inventory categories
        /// </summary>
        public ObservableCollection<ItemCategoryViewModel> ItemCategories { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            ItemCategories = new ObservableCollection<ItemCategoryViewModel>();
        }
    }
}
