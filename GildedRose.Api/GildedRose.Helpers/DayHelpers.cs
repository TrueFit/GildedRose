using GildedRose.BusinessObjects;
using GildedRose.Helpers.ItemTypes;

namespace GildedRose.Helpers
{
    /// <summary>
    /// The purpose of this class is to allow for the factories to end the day for
    /// each type of item we have.  This is where the business logic occurs.
    /// I decided to use factories so that if new item types with new conditions are 
    /// needed, we extend the number of classes that implement IItemType.  This way
    /// we don't have to worry about violating the open-close principle.
    /// 
    /// Also, at the helpers level, we do not need to access lower levels to retrieve data.
    /// The data is handed to us by the services layer, so that the business logic
    /// becomes easy to test.
    /// </summary>
    public static class DayHelpers
    {
        public static InventoryItem EndDay(InventoryItem i)
        {
            var factory = new InventoryItemTypeFactory();

            var itemType = factory.CreateInstance(i);
            return itemType.EndDay(i);
        }
    }
}
