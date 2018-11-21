using System.Runtime.CompilerServices;
using GildedRose.Data.Interfaces;
using GildedRose.Model;

namespace GildedRose.Services.Items
{
    public class Armor : StandardBusinessObject
    {
        public Armor(IStoreItemRepository itemsRepository) : base(itemsRepository)
        {
        }
    }
}