using System.Runtime.CompilerServices;
using GildedRose.Data.Interfaces;
using GildedRose.Model;

namespace GildedRose.Services.Items
{
    public class Food : StandardBusinessObject
    {
        public Food(IStoreItemRepository itemsRepository) : base(itemsRepository)
        {
        }

        public override void AdjustQuality(StoreItemDto item)
        {
            if (item.Name == "Aged Brie")
                // Aged Brie actually increases in Quality the older it gets.  There are no other 
                // stated constraints other than the Quality limit so we'll keep increasing the quality to the limit
                item.Quality++;
            else
                item.Quality--;
        }

    }
}