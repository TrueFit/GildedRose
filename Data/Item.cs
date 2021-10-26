using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace GildedRose.Data
{
    public interface IItem
    {
        string Name { get; set; }
        string Category { get; set; }
        int SellIn { get; set; }
        int Quality { get; set; }
        string ToString();
        void UpdateItem();
    }

    public class Item : IItem
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int SellIn { get; set; }
        public int Quality { get; set; }
        public Item(){}
        public Item(string name, string category, int sellIn, int quality)
        {
            Name = name;
            Category = category;
            SellIn = sellIn;
            Quality = quality;
        }
        public override string ToString()
        {
            return Name + ", " + Category + ", " + SellIn + ", " + Quality;
        }
        public virtual void UpdateItem()
        {
            // decrease SellIn by one day
            LowerDaysLeft();
            // decrease item quality
            LowerQuality();
            // quality can be neither too high nor too low
            ApplyQualityLimit();
        }

        protected void LowerDaysLeft()
        {
            SellIn--;
        }

        protected virtual void LowerQuality()
        {
            // Once the sell by date has passed...
            if (SellIn < 0)
            {
                // ... Quality degrades twice as fast
                Quality -= 2;
            }
            else
            {
                Quality--;
            }
        }

        protected void ApplyQualityLimit()
        {
            //The Quality of an item is never negative
            if (Quality < 0)
            {
                Quality = 0;
            }
            // An item can never have its Quality increase above 50
            if (Quality > 50)
            {
                Quality = 50;
            }
        }
    }
}