using System;
using System.Threading.Tasks;
using GildedRose.Logic.Repo;
using Microsoft.AspNetCore.Mvc;

namespace GildedRose.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ItemRepo itemRepo;

        public ItemController(ItemRepo itemRepo)
        {
            this.itemRepo = itemRepo;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll(string viewByDate)
        {
            if (DateTime.TryParse(viewByDate, out DateTime outputDate))
            {
                var allItems = await this.itemRepo.GetAll(outputDate);
                return this.Ok(allItems);
            }

            return this.BadRequest();
        }

        [HttpGet]
        [Route("name/{itemName}")]
        public async Task<IActionResult> GetByName(string itemName)
        {
            var allItems = await this.itemRepo.GetAll(DateTime.Now);
            return this.Ok(allItems);
        }

        [HttpGet]
        [Route("category/{categoryId}")]
        public async Task<IActionResult> GetAll(int categoryId)
        {
            var allItems = await this.itemRepo.GetAll(DateTime.Now);
            return this.Ok(allItems);
        }
    }
}