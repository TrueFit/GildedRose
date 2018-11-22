using System.Threading.Tasks;
using GildedRose.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GildedRose.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemManager itemManager;

        public ItemController(IItemManager itemManager)
        {
            this.itemManager = itemManager;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var allItems = await this.itemManager.GetAll();
            return this.Ok(allItems);
        }

        [HttpGet]
        [Route("name/{itemName}")]
        public async Task<IActionResult> GetByName(string itemName)
        {
            var items = await this.itemManager.GetByName(itemName);
            return this.Ok(items);
        }

        [HttpGet]
        [Route("category/{categoryId}")]
        public async Task<IActionResult> GetAll(int categoryId)
        {
            var items = await this.itemManager.GetByCategory(categoryId);
            return this.Ok(items);
        }
    }
}