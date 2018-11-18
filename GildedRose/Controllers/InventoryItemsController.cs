using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GildedRose.Persistence.Context;
using GildedRose.Models;
using GildedRose.Persistence;
using GildedRose.Persistence.Database;
using AutoMapper;
using GildedRose.Domain.Models;
using GildedRose.Domain;

namespace GildedRose.Inventory.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Items")]
    public class InventoryItemsController : Controller
    {
        private readonly InventoryContext _context;
        private readonly IDatabase<InventoryItem> _items;
        private readonly InventoryItemDomain _itemDomain;
        private readonly IMapper _mapper;

        public InventoryItemsController(InventoryContext context, IMapper mapper, IDatabase<InventoryItem> items, IDomain<InventoryItemValue> itemDomain)
        {
            _context = context;
            _mapper = mapper;
            _items = items;
            _itemDomain = (InventoryItemDomain)itemDomain;
        }

        // GET: api/Items
        [HttpGet]
        public IEnumerable<InventoryItemValue> GetItems(int inventoryId)
        {
            return _itemDomain.GetInventoryItems(inventoryId);
        }

        // GET: api/Items
        [HttpGet("Search")]
        public IEnumerable<InventoryItemValue> SearchItems(int inventoryId, string itemName)
        {
            return _itemDomain.SearchInventoryItems(inventoryId, itemName);
        }

        // GET: api/Items
        [HttpGet("Trash")]
        public IEnumerable<InventoryItemValue> GetTrashItems()
        {
            return _itemDomain.GetTrashItems();
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public InventoryItemValue GetItem(int id)
        {
            return _itemDomain.GetInventoryItem(id);
        }

        // POST: api/Items
        [HttpPost]
        public void AddItem([FromBody]InventoryItemValue item)
        {
            _itemDomain.AddInventoryItem(item);
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public void PutInventory([FromBody]InventoryItemValue item)
        {
            _itemDomain.AddInventoryItem(item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public void DeleteItem(int itemId)
        {
            _itemDomain.DeleteInventoryItem(itemId);
        }
    }
}
