using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GildedRose.Domain.Entity
{
    public class Conjured : Item
    {
        public override void AdjustQuality()
        {
            base.AdjustQuality();
            base.AdjustQuality();
        }
    }
}