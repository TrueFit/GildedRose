using GildedRose.Data.Interfaces;
using GildedRose.Model;

namespace GildedRose.Services.Items
{
    public class Potion : StandardBusinessObject
    {
        public Potion(IStoreItemRepository itemsRepository) : base(itemsRepository)
        {
        }
    }
}