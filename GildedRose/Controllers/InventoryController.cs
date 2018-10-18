using Autofac.Integration.WebApi;
using GildedRose.Domain.Entity;
using GildedRose.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GildedRose.Controllers
{
    [AutofacControllerConfiguration]
    public class InventoryController : ApiController
    {
        private IInventoryRepository _inventoryRepo;

        public InventoryController(IInventoryRepository repo)
        {
            this._inventoryRepo = repo;
        }

        // GET: api/Inventory
        [HttpGet]
        [ResponseType(typeof(IList<Item>))]
        public IList<Item> Get()
        {
            IList<Item> items = _inventoryRepo.All();
            return items;
        }

        // GET: api/Inventory/5
        [HttpGet]
        [ResponseType(typeof(Item))]
        public Item Get(string name)
        {
            Item item = _inventoryRepo.GetByName(name);
            return item;
        }

        [HttpGet]
        [ResponseType(typeof(IList<Item>))]
        public IList<Item> Trash()
        {
            IList<Item> items = _inventoryRepo.GetByCondition(i => i.Quality == 0);
            return items;
        }

        [HttpPost]
        [ResponseType(typeof(IList<Item>))]
        public IList<Item> EndDay()
        {
            IList<Item> items = _inventoryRepo.All();
            foreach (Item item in items)
            {
                item.IncrementAge();
            }
            _inventoryRepo.Save(items);
            return items;
        }

        // DELETE: api/Inventory/5
        public void Delete(int id)
        {
        }
    }
}
