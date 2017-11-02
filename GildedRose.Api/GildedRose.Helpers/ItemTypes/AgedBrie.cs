using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GildedRose.BusinessObjects;

namespace GildedRose.Helpers.ItemTypes
{
    public class AgedBrie : IItemType
    {
        public InventoryItem EndDay(InventoryItem i)
        {
            // "Aged Brie" actually increases in Quality the older it gets

            // Sellin is normal, subtract until it's zero
            i.Sellin = (i.Sellin - 1 > 0) ? i.Sellin - 1 : 0;

            var qualityFactor = 1;
            // Once the sell by date has passed, Quality degrades twice as fast 
            // Assumption:  for Brie, the Quality should increase twice as fast, make it extra stinky
            if (i.Sellin == 0) qualityFactor = 2;
            i.Quality = (i.Quality < 50) ? i.Quality + qualityFactor : 50;
            return i;
        }
    }
}
