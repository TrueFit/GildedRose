using GildedRose.Data.Interfaces;
using GildedRose.Model;

namespace GildedRose.Services.Items
{
    public class Sulfuras : StandardBusinessObject
    {
        public Sulfuras(IStoreItemRepository itemsRepository) : base(itemsRepository)
        {
        }
    
        //Sulfuras is a legendary item and as such its Quality is 80 and it never alters or decreases in Quality.
        private const int LegendaryQuality = 80;
        //Sulfuras is a legendary item, never has to be sold 
        private const int NeverSellIn = 80;

        ///-------------------------------------------------------------------------------------------------
        /// <summary> Gets or sets the AdjustSellIn value which is the number of days an item must be sold in.  
        ///           Zero essentially means the items expired. </summary>
        ///
        /// <value> The number of days to sell the item by. </value>
        /// <remarks> The AdjustSellIn date can be "set" (see the inventory list) but the value is essentially 
        ///           ignored because never has to be sold.</remarks>
        ///-------------------------------------------------------------------------------------------------
        public override void AdjustSellIn(StoreItemDto item)
        {
            item.SellIn = NeverSellIn;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>  Gets or sets the Quality. While most item can never have a Quality higher than 50, 
        ///            Sulfuras is a legendary item and as such its Quality is 80 and it is never altered.
        ///            </summary>
        ///
        /// <value> The quality as a constant. </value>
        /// <remarks> The Quality date can be "set" (see the inventory list) but the value is essentially 
        ///           ignored because never decreases in Quality.</remarks>
        ///-------------------------------------------------------------------------------------------------

        public override void AdjustQuality(StoreItemDto item)
        {
            item.SellIn = LegendaryQuality;
        }

    }
}