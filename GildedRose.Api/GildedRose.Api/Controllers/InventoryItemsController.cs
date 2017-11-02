using GildedRose.BusinessObjects;
using GildedRose.Entities;
using GildedRose.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace GildedRose.Api.Controllers
{
    [RoutePrefix("api/InventoryItems")]
    public class InventoryItemsController : ApiController
    {
        private readonly InventoryItemServices _service;

        public InventoryItemsController(GildedRoseEntities context)
        {
            _service = new InventoryItemServices(context);
        }

        public InventoryItemsController()
        {
            var context = new GildedRoseEntities();
            _service = new InventoryItemServices(context);
        }

        // GET api/InventoryItems/items
        [HttpGet]
        [Route("items")]
        public IEnumerable<InventoryItem> Get() => _service.GetAllItems();
        
        // GET api/InventoryItems/item/{name}
        [HttpGet]
        [Route("item/{name}")]
        public InventoryItem Get(string name) => _service.Find(name);

        // GET api/InventoryItems/Zero
        [HttpGet]
        [Route("Zero")]
        public IEnumerable<InventoryItem> GetZeroItems() => _service.GetZeroQualityItems();

        // GET api/InventoryItems/InitialState
        [HttpGet]
        [Route("InitialState")]
        public IEnumerable<InventoryItem> Reset() => _service.ResetInitialInventory();

    }
}
