using GildedRose.Client.Models;

namespace GildedRose.Client.ViewModels
{
    /// <summary>
    /// View model for an item
    /// </summary>
    public class ItemViewModel : AViewModel<IItemModel>
    {
        #region Name

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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
            Name = Model.Name;
            SellIn = Model.SellIn;
            Quality = Model.Quality;

            Model.PropertyChanged += (sender, e) =>
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
            };
        }
    }
}
