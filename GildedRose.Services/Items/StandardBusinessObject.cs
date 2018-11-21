using System;
using GildedRose.Data.Interfaces;
using GildedRose.Model;

namespace GildedRose.Services.Items
{
    public interface IServiceActionBase
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>  Gets or sets the Quality. Items can never have a Quality higher than the MaxQuality
        ///            50 and the Quality of an item is never negative.
        ///            </summary>
        ///
        /// <value> The Quality value between the MinQuality and MaxQuality value inclusive. </value>
        ///-------------------------------------------------------------------------------------------------
        void Age(StoreItemDto item);

        void AdjustQuality(StoreItemDto item);
        void AdjustSellIn(StoreItemDto item);
        void QualityCheck(StoreItemDto item);
    }

    public class StandardBusinessObject : IServiceActionBase
    {
        private readonly IStoreItemRepository _itemsRepository;
        private const int MaxQuality = 50;
        private const int MinQuality = 0;

        public StandardBusinessObject(IStoreItemRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>  Gets or sets the Quality. Items can never have a Quality higher than the MaxQuality
        ///            50 and the Quality of an item is never negative.
        ///            </summary>
        ///
        /// <value> The Quality value between the MinQuality and MaxQuality value inclusive. </value>
        ///-------------------------------------------------------------------------------------------------

        public void Age(StoreItemDto item)
        {
            AdjustSellIn(item);

            AdjustQuality(item);

            QualityCheck(item);

            _itemsRepository.Update(item);
        }

        public virtual void AdjustQuality(StoreItemDto item)
        {
            // Once the sell by date has passed, Quality degrades twice as fast
            if (item.SellIn == 0)
                item.Quality = item.Quality - 2;
            else
                item.Quality--;
        }

        public virtual void AdjustSellIn(StoreItemDto item)
        {
            item.SellIn--;
        }

        public virtual void QualityCheck(StoreItemDto item)
        {
                // The Quality of an item is never negative
                if (item.Quality < 0)
                {
                    item.Quality = 0;
                }
                //The Quality of an item is never more than 50
                else if (item.Quality > 50)
                {
                    item.Quality = 50;
                }
            }
    }
}

