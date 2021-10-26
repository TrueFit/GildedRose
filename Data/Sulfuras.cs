using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// ReSharper disable IdentifierTypo

namespace GildedRose.Data
{
    // TODO - deal with the Sellin override
    public class Sulfuras : Item
    {
        // Sulfuras", being a legendary item, never has to be sold or decreases in Quality
        // (Quality is 80 and it never alters)
        public Sulfuras(string name, string category, int sellIn, int quality)
            : base(name, category, sellIn, quality) { }

        public override void UpdateItem()
        {
            // "Sulfuras" is a legendary item and as such its Quality is 80 and it never alters.
            Quality = 80;
            //never has to be sold.  i.e. sellIn = sellIn
        }
    }
}
