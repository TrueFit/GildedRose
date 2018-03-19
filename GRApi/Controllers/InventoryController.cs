using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GR.BusinessLogic;
using GR.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;

namespace GRApi.Controllers {
    [Route("[controller]")]
    public class InventoryController : Controller {
        // GET inventory
        [HttpGet]
        public IEnumerable<Item> Get() {
            Inventory invenotry = new Inventory();
            return invenotry.GetAllItems().ToList();
        }

        // GET inventory/Item Name
        [HttpGet("/inventory/name/{itemName}")]
        public IActionResult Get(string itemName) {
            Inventory invenotry = new Inventory();

            var item = invenotry.GetItem(itemName);
            if (item == null) {
                return new NoContentResult();
            }
            return new ObjectResult(item);
        }

        // GET inventory/trash
        [HttpGet]
        [Route("trash")]
        public IEnumerable<Item> Trash() {
            Inventory invenotry = new Inventory();
            return invenotry.GetTrashList().ToList();
        }

        [HttpPost()]
        [Route("end-of-day")]
        public IActionResult EndOfDay() {
            Inventory inventory = new Inventory();
            inventory.EndTheDay();

            return CreatedAtRoute("inventory", null);
        
        }
    }
}