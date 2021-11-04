using GildedRose.Client.ViewModels;
using System.Windows;

namespace GildedRose.Client
{
    /// <summary>
    /// The application's main window
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            // Create view model.
            _mainWindowViewModel = new MainWindowViewModel();

            // Initialize view.
            InitializeComponent();

            DataContext = _mainWindowViewModel;

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.Initialize();
        }
    }
}
