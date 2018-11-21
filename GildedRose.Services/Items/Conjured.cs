using GildedRose.Data.Interfaces;
using GildedRose.Model;

namespace GildedRose.Services.Items
{
    public class Conjured : StandardBusinessObject
    {
        public Conjured(IStoreItemRepository itemsRepository) : base(itemsRepository)
        {
        }
        public override void AdjustQuality(StoreItemDto item)
        {
            // Conjured items degrade in Quality twice as fast as normal items
            if (item.SellIn == 0)
                item.Quality = item.Quality - 4;
            else
                item.Quality = item.Quality - 2;
        }

    }
}