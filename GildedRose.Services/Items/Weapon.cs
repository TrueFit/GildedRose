using GildedRose.Data.Interfaces;
using GildedRose.Model;

namespace GildedRose.Services.Items
{
    class Weapon : StandardBusinessObject
    {
        public Weapon(IStoreItemRepository itemsRepository) : base(itemsRepository)
        {
        }
    }
}
