using CsvHelper.Configuration.Attributes;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GildedRoseInventory
{
    public class Product: IComparable<Product>
    {
        [Index(0)]
        public string Name { get; set; }

        [Index(1)]
        public string Category { get; set; }

        [Index(2)]
        public int SellIn { get; set; }

        [Index(3)]
        public int Quality { get; set; }

        public int CompareTo([AllowNull] Product other)
        {
            // A null value means that this object is greater.
            if (other == null)
                return 1;

            // if the category matches, fall to the name
            var comp = Category.CompareTo(other.Category);
            if (comp == 0)
                return Name.CompareTo(other.Name);

            return comp;

        }
    }
}
