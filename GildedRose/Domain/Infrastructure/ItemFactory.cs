using GildedRose.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GildedRose.Domain.Infrastructure
{
    public abstract class ItemFactory
    {
        public const string AGEDBRIE_NAME = "Aged Brie";
        public const string FOOD_CATEGORY = "Food";
        public const string BACKSTAGEPASSES_CATEGORY = "Backstage Passes";
        public const string CONJURED_CATEGORY = "Conjured";
        public const string SULFURAS_CATEGORY = "Sulfuras";

        public abstract Item GetItem(string name, string category, int sellIn, int quality);
    }
}
