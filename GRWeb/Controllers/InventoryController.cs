using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GR.BusinessLogic;
using GR.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;

namespace GRWeb.Controllers {
    [Route("api/[controller]")]
    public class Inventory : Controller {
        [HttpGet]
        public IEnumerable<Item> Get() {
            GR.BusinessLogic.Inventory invenotry = new GR.BusinessLogic.Inventory();
            return invenotry.GetAllItems().ToList();
        }

        // GET inventory/Item Name
        [HttpGet("item/{itemName}")]
        public IActionResult Get(string itemName) {
            GR.BusinessLogic.Inventory invenotry = new GR.BusinessLogic.Inventory();

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
            GR.BusinessLogic.Inventory invenotry = new GR.BusinessLogic.Inventory();
            return invenotry.GetTrashList().ToList();
        }

        [HttpPost()]
        [Route("end-of-day")]
        public IActionResult EndOfDay() {
            GR.BusinessLogic.Inventory inventory = new GR.BusinessLogic.Inventory();
            inventory.EndTheDay();

            return Ok();

        }
    }
}