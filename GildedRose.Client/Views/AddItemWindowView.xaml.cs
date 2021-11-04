using GildedRose.Client.ViewModels;
using System.Windows;

namespace GildedRose.Client.Views
{
    /// <summary>
    /// Dialog window to add a new item
    /// </summary>
    public partial class AddItemWindowView : Window
    {
        private readonly AddItemWindowViewModel _viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="viewModel">Dialog view model</param>
        public AddItemWindowView(AddItemWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.Window = this;

            InitializeComponent();

            DataContext = _viewModel;
        }
    }
}
