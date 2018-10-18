using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GildedRose.Domain.Entity
{
    public class BackstagePasses : Item
    {
        public override void AdjustQuality()
        {
            if (SellIn <= 0)
                Quality = 0;
            else if (SellIn <= 5)
                Quality += 3;
            else if (SellIn <= 10)
                Quality += 2;
            else
                Quality++;
        }
    }
}