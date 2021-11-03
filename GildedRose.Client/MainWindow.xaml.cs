using GildedRose.Client.IO;
using GildedRose.Client.Models;
using GildedRose.Client.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GildedRose.Client
{
    /// <summary>
    /// The application's main window
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            // Import inventory list.
            var items = InventoryFileImport.ImportItems(@"C:\Projects\Gilded Rose\trunk\inventory.txt", out var errors);

            // Sort items into categories.
            var categories = new List<ItemCategoryModel>();
            foreach (var item in items)
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

            // Create view model.
            var mainWindowViewModel = new MainWindowViewModel();

            foreach (var category in categories)
                mainWindowViewModel.ItemCategories.Add(new ItemCategoryViewModel(category));

            // Initialize view.
            InitializeComponent();

            DataContext = mainWindowViewModel;
        }
    }
}
