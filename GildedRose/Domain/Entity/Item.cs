using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GildedRose.Domain.Entity
{
    public class Item
    {
        private int quality;
        public virtual string Name { get; set; }
        public virtual string Category { get; set; }
        public virtual int SellIn { get; set; }
        public virtual int Quality {
            get { return quality; }
            set {
                quality = value;
                if (quality < 0)
                    quality = 0;
                if (quality > 50)
                    quality = 50;
            }
        }

        public Item() { }

        public void IncrementAge()
        {
            AdjustQuality();
            --SellIn;
        }

        public virtual void AdjustQuality()
        {
            if (SellIn <= 0)
                Quality -= 2;
            else
                Quality--;
        }

        public override string ToString()
        {
            return String.Format("{0},{1},{2},{3}", Name, Category, SellIn, Quality);
        }
    }
}