using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GildedRose.Domain.Entity
{
    public class AgedBrie : Item
    {
        public override void AdjustQuality()
        {
            if (SellIn <= 0)
                Quality += 2;
            else
                Quality++;
        }
    }
}