using GildedRose.Data;
using GildedRose.Services;

namespace GildedRose.Tests
{
    public class ItemTestsBase
    {
        protected IItemFactory Factory;
        protected IItem Item;

        public void UpdateItemMultipleTimes(IItem item, int passes = 1)
        {
            for (int i = 0; i < passes; i++)
            {
                item.UpdateItem();
            }
        }
    }
}