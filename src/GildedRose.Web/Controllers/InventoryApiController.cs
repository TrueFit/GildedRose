using GildedRose.Domain;
using GildedRose.Repository;
using GildedRose.Web.Models.InventoryApi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRose.Web.Controllers
{
    [Route("api/inventoryItems")]
    [ApiController]
    public class InventoryApiController : ControllerBase
    {
        private readonly IInventoryItemBuilder inventoryItemBuilder;
        private readonly IInventoryRepository repository;

        public InventoryApiController(IInventoryItemBuilder inventoryItemBuilder, IInventoryRepository repository)
        {
            this.inventoryItemBuilder = inventoryItemBuilder;
            this.repository = repository;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItem([FromBody]AddModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            IInventoryItem item = this.inventoryItemBuilder.Build(Guid.NewGuid(), model.Name, model.Category, model.Quality, model.SellIn);

            await this.repository.AddItemAsync(item);

            return new JsonResult(item);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<IInventoryItem> items = await this.repository.GetAllAsync();
            return new JsonResult(items.ToArray());
        }

        [HttpGet("query/{name}")]
        public async Task<IActionResult> GetByName([FromRoute]string name)
        {
            IEnumerable<IInventoryItem> items = await this.repository.GetAllAsync();
            items = items.Where(_ => string.Equals(name, _.Name, StringComparison.CurrentCultureIgnoreCase));
            return new JsonResult(items.ToArray());
        }

        [HttpGet("trash")]
        public async Task<IActionResult> GetTrash()
        {
            IEnumerable<IInventoryItem> items = await this.repository.GetAllAsync();
            items = items.Where(_ => _.Quality <= 0);
            return new JsonResult(items.ToArray());
        }

        [HttpPost("nextDay")]
        public async Task<IActionResult> NextDay()
        {
            IEnumerable<IInventoryItem> items = await this.repository.GetAllAsync();
            foreach (IInventoryItem item in items)
            {
                item.OnAdvanceToNextDay();
            }

            await this.repository.SaveAsync(items);

            return new JsonResult(items.ToArray());
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> RemoveItem([FromRoute]Guid id)
        {
            bool removed = await this.repository.RemoveItemAsync(id);
            if (removed)
            {
                return new OkResult();
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [HttpPost("reset")]
        public async Task<IActionResult> Reset()
        {
            await this.repository.ResetAsync();
            return new OkResult();
        }
    }
}