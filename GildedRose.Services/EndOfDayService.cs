using System.Collections.Generic;
using System.Linq;
using GildedRose.Data.Interfaces;
using GildedRose.Model;
using GildedRose.Services.Interfaces;

namespace GildedRose.Services
{
    public class EndOfDayService : IEndOfDayService
    {
        private readonly IStoreItemRepository _itemRepository;
        private readonly ItemFactory _itemFactory;

        public EndOfDayService(IStoreItemRepository itemsRepository)
        {
            this._itemRepository = itemsRepository;
            _itemFactory = new ItemFactory(itemsRepository);
        }

        public List<StoreItemDto> GetListofTrash()
        {
            // get a list of all items with a AdjustSellIn value of zero (0)
            var trashItems = _itemRepository.GetListofTrash().ToList();
            return trashItems;
        }

        public void ProcessEndOfDay()
        {
            // The end of day process will be requested of the service object.
            // use a factory here to instantiate an object that we can act on (maybe an IOC container 
            // or a factory the scans this assembly for new types).  The end of day process should 
            // decrement the AdjustSellIn property and alter the Quality appropriately and then call the 
            // repository to update.
            foreach (var item in _itemRepository.GetAll())
            {
                var businessObject = _itemFactory.GetBusinessObject(item);
                businessObject.Age(item);
            }
        }

        public void SaveChanges()
        {
            _itemRepository.Save();
        }
    }
}