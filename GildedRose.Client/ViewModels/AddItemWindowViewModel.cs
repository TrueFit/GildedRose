using System.Windows;
using System.Windows.Input;

namespace GildedRose.Client.ViewModels
{
    /// <summary>
    /// View model for a dialog window to add a new item
    /// </summary>
    public class AddItemWindowViewModel : AViewModel
    {
        #region Name

        /// <summary>
        /// The item's name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                ValidateInput();
                NotifyPropertyChanged(nameof(Name));
            }
        }

        private string _name;

        #endregion

        #region Category

        /// <summary>
        /// The item's category
        /// </summary>
        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                ValidateInput();
                NotifyPropertyChanged(nameof(Category));
            }
        }

        private string _category;

        #endregion

        #region SellIn

        /// <summary>
        /// The item's sell-in counter
        /// </summary>
        public int SellIn
        {
            get { return _sellIn; }
            set
            {
                _sellIn = value;
                ValidateInput();
                NotifyPropertyChanged(nameof(SellIn));
            }
        }

        private int _sellIn;

        #endregion

        #region Quality

        /// <summary>
        /// The item's initial quality
        /// </summary>
        public int Quality
        {
            get { return _quality; }
            set
            {
                _quality = value;
                ValidateInput();
                NotifyPropertyChanged(nameof(Quality));
            }
        }

        private int _quality;

        #endregion

        #region CanBeAdded

        /// <summary>
        /// Can the new item be added? Is it valid?
        /// </summary>
        public bool CanBeAdded
        {
            get { return _canBeAdded; }
            set
            {
                _canBeAdded = value;
                NotifyPropertyChanged(nameof(CanBeAdded));
            }
        }

        private bool _canBeAdded;

        #endregion

        /// <summary>
        /// Command to approve the changes and add the new item
        /// </summary>
        public SimpleCommand OkayCommand { get; }

        /// <summary>
        /// Command to cancel this action
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Reference to the window from the voew
        /// </summary>
        public Window Window { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AddItemWindowViewModel()
        {
            OkayCommand = new SimpleCommand(() =>
            {
                Window.DialogResult = true;
                Window.Close();
            });

            CancelCommand = new SimpleCommand(() =>
            {
                Window.DialogResult = false;
                Window.Close();
            });
        }

        private void ValidateInput()
        {
            CanBeAdded = !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Category) && Quality > 0 && SellIn > 0;
        }
    }
}
