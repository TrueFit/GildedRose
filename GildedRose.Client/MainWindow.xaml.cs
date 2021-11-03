using GildedRose.Client.IO;
using System.Windows;

namespace GildedRose.Client
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var models = InventoryFileImport.ImportItems(@"C:\Projects\Gilded Rose\trunk\inventory.txt", out var errors);

            InitializeComponent();
        }
    }
}
