using GildedRose.Client.Models;
using System;

namespace GildedRose.Client.ViewModels
{
    /// <summary>
    /// View model for an item
    /// </summary>
    public class ItemViewModel : AViewModel<IItemModel>, IDisposable
    {
        #region Name

        /// <summary>
        /// An item's name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        private string _name;

        #endregion

        #region SellIn

        /// <summary>
        /// The SellIn value denotes the number of days the store owners have to sell the item.
        /// </summary>
        public int SellIn
        {
            get { return _sellIn; }
            set
            {
                _sellIn = value;
                NotifyPropertyChanged(nameof(SellIn));
            }
        }

        private int _sellIn;

        #endregion

        #region Quality

        /// <summary>
        /// The quality of an item states how valuable the item is.
        /// </summary>
        public int Quality
        {
            get { return _quality; }
            set
            {
                _quality = value;
                NotifyPropertyChanged(nameof(Quality));
            }
        }

        private int _quality;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item">The item</param>
        public ItemViewModel(IItemModel item)
            : base(item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            Name = Model.Name;
            SellIn = Model.SellIn;
            Quality = Model.Quality;

            // Make sure we got notified about model changes.
            Model.PropertyChanged += Model_PropertyChanged;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (Model != null)
                Model.PropertyChanged -= Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IItemModel.Name):
                    Name = Model.Name;
                    break;

                case nameof(IItemModel.SellIn):
                    SellIn = Model.SellIn;
                    break;

                case nameof(IItemModel.Quality):
                    Quality = Model.Quality;
                    break;
            }
        }
    }
}
