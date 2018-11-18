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
    [Route("api/v1/Inventories")]
    public class InventoryController : Controller
    {
        private readonly InventoryContext _context;
        private readonly IDatabase<Models.Inventory> _inventories;
        private readonly InventoryDomain _inventoryDomain;
        private readonly IMapper _mapper;

        public InventoryController(InventoryContext context, IMapper mapper, IDatabase<Models.Inventory> inventories, IDomain<InventoryValue> inventoryDomain)
        {
            _context = context;
            _mapper = mapper;
            _inventories = inventories;
            _inventoryDomain = (InventoryDomain)inventoryDomain;
        }

        // GET: api/Inventory
        [HttpGet]
        public IEnumerable<InventoryValue> GetInventories()
        {
            return _inventoryDomain.GetInventories();
        }

        // GET: api/Inventory/5
        [HttpGet("{id}")]
        public InventoryValue GetInventory(int id)
        {
            return _inventoryDomain.GetInventory(id);
        }

        // POST: api/Inventory
        [HttpPost]
        public void AddInventory([FromBody]InventoryValue value)
        {
            _inventoryDomain.AddInventory(value);
        }

        // POST: api/Inventory/Default
        [HttpPost("Default")]
        public void AddInventoryDefault([FromBody]InventoryValue inventory)
        {
            _inventoryDomain.PurchaseDefaultInventory(inventory);
        }

        // POST: api/Inventory/{id}/Advance
        [HttpPost("{id}/Advance")]
        public void AdvanceDay(int id)
        {
            _inventoryDomain.AdvanceDay(id);
        }

        // PUT: api/Inventory/5
        [HttpPut("{id}")]
        public void PutInventory(int id, [FromBody]InventoryValue value)
        {
            _inventoryDomain.AddInventory(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteInventory(int id)
        {
            _inventoryDomain.DeleteInventory(id);
        }
    }
}
