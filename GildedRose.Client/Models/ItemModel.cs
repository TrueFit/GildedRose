using System;

namespace GildedRose.Client.Models
{
    /// <summary>
    /// An item that is available for purchase in the Gilded Rose.
    /// </summary>
    public class ItemModel : AModel, IItemModel
    {
        /// <inheritdoc />
        public Guid Id { get; set; }

        #region Name

        /// <inheritdoc />
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _name;

        #endregion

        #region Category

        /// <inheritdoc />
        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        private string _category;

        #endregion

        #region SellIn

        /// <inheritdoc />
        public int SellIn
        {
            get { return _sellIn; }
            set
            {
                _sellIn = value;
                OnPropertyChanged(nameof(SellIn));
            }
        }

        private int _sellIn;

        #endregion

        #region Quality

        /// <inheritdoc />
        public int Quality
        {
            get { return _quality; }
            set
            {
                _quality = value;
                OnPropertyChanged(nameof(Quality));
            }
        }

        private int _quality;

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Id = {Id}, Name = \"{Name}\", Category = \"{Category}\", SellIn = {SellIn}, Quality = {Quality}";
        }
    }
}
