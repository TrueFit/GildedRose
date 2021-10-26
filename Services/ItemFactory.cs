using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GildedRose.Data;

namespace GildedRose.Services
{
    // TODO - swap this out with a more sophisticated factory that does not violate OCP
    // this is a very naive parameterized factory implementation.  The switch statement is a code smell and does not
    // support the Open Closed Principle when adding new custom Item types but at least all the object creation code
    // is in one spot.
    public interface IItemFactory
    {
        //C# 8.0 lets us take advantage of static methods in interface so long as we declare a default body for them
        static ItemFactory Instance => throw new NotImplementedException();
        IItem CreateItem(string name, string category, int sellIn, int quality) => throw new NotImplementedException();
    }

    public class ItemFactory : IItemFactory
    {
        public static ItemFactory Instance { get; } = new ItemFactory();

        public IItem CreateItem(string name, string category, int sellIn, int quality)
        {
            IItem item;

            switch (category)
            {
                case "Food":
                    if (name == "Aged Brie")
                        item = new AgedBrie(name, category, sellIn, quality);
                    else
                        item = new Item(name, category, sellIn, quality);
                    break;
                case "Sulfuras":
                    item = new Sulfuras(name, category, sellIn, quality);
                    break;
                case "Backstage Passes":
                    item = new BackstagePass(name, category, sellIn, quality);
                    break;
                case "Conjured":
                    item = new ConjuredItem(name, category, sellIn, quality);
                    break;
                case "Armor":
                case "Misc":
                case "Potion":
                case "Weapon":
                    item = new Item(name, category, sellIn, quality);
                    break;
                default:
                    // TODO - this could be a custom exception class instance
                    throw new KeyNotFoundException("Category " + category + " is not supported.");
            }

            return item;
        }
    }
}
