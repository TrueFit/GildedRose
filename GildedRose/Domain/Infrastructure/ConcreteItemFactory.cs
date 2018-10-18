using GildedRose.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GildedRose.Domain.Infrastructure
{
    public class ConcreteItemFactory : ItemFactory
    {
        private Item GetItem(string category)
        {
            Item item;
            switch(category)
            {
                case AGEDBRIE_NAME:
                    item = new AgedBrie();
                    break;
                case BACKSTAGEPASSES_CATEGORY:
                    item = new BackstagePasses();
                    break;
                case CONJURED_CATEGORY:
                    item = new Conjured();
                    break;
                case SULFURAS_CATEGORY:
                    item = new Sulfuras();
                    break;
                default:
                    item = new Item();
                    break;
            }
            item.Category = category;
            return item;
        }

        private Item GetItem(string category, string name)
        {
            Item item;
            switch (category)
            {
                case FOOD_CATEGORY:
                    item = GetItem(name);
                    break;
                default:
                    item = GetItem(category);
                    break;
            }
            return item;
        }

        public override Item GetItem(string name, string category, int sellIn, int quality)
        {
            Item item = GetItem(category, name);
            item.Name = name;
            item.Quality = quality;
            item.SellIn = sellIn;
            return item;
            //switch(category)
            //{
            //    case AGEDBRIE_CATEGORY:
            //        AgedBrie agedBrie = new AgedBrie
            //        {
            //            Category = category,
            //            Name = name,
            //            SellIn = sellIn,
            //            Quality = quality
            //        };
            //        return agedBrie;
            //    case BACKSTAGEPASSES_CATEGORY:
            //        return new BackstagePasses
            //        {
            //            Category = category,
            //            Name = name,
            //            SellIn = sellIn,
            //            Quality = quality
            //        };
            //    case CONJURED_CATEGORY:
            //        return new Conjured
            //        {
            //            Category = category,
            //            Name = name,
            //            SellIn = int.Parse(sellIn),
            //            Quality = int.Parse(quality)
            //        };
            //    case SULFURAS_CATEGORY:
            //        return new Sulfuras
            //        {
            //            Category = category,
            //            Name = name,
            //            SellIn = int.Parse(sellIn),
            //            Quality = int.Parse(quality)
            //        };
            //    default:
            //        return new Item
            //        {
            //            Category = category,
            //            Name = name,
            //            SellIn = int.Parse(sellIn),
            //            Quality = int.Parse(quality)
            //        };
            //}
        }
    }
}