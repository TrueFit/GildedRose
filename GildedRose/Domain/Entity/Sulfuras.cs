using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GildedRose.Domain.Entity
{
    public class Sulfuras : Item
    {
        private int quality = 80;
        public override int Quality
        {
            get { return quality; }
            set { quality = 80; }
        }
        public override void AdjustQuality()
        {
            return;
        }
    }
}