using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuildedRose.Inventory.Data;
using GuildedRose.Inventory.InventoryItems;
using GuildedRose.Inventory.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuildedRose.Inventory.Controllers
{
    /// <summary>
    /// The API controller for the Inventory page
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryIncrementDayService _InventoryIncrementDayService;
        private readonly InventoryDbContext _InventoryDbContext;

        /// <summary>
        /// Constructor for <see cref="InventoryController"/>
        /// </summary>
        /// <param name="inventoryIncrementDayService">Current <see cref="_InventoryIncrementDayService"/></param>
        public InventoryController(InventoryDbContext inventoryDbContext, InventoryIncrementDayService inventoryIncrementDayService)
        {
            _InventoryDbContext = inventoryDbContext;
            _InventoryIncrementDayService = inventoryIncrementDayService;
        }

        /// <summary>
        /// Main GET handler for the Inventory page
        /// </summary>
        /// <returns><see cref="IEnumerable{InventoryItem}"/> representing current inventory items</returns>
        [HttpGet]
        public async Task<IEnumerable<InventoryItem>> GetAsync()
        {
            return await GetInventoryItemsAsync();
        }

        /// <summary>
        /// Gets current <see cref="InventoryItem"/>s from the database
        /// </summary>
        /// <returns><see cref="IEnumerable{InventoryItem}"/> representing current inventory items</returns>
        private async Task<IEnumerable<InventoryItem>> GetInventoryItemsAsync()
        {
            var inventoryItems = await _InventoryDbContext.Set<InventoryItem>().ToListAsync();
            return inventoryItems.OrderBy(i => i.Category).ThenBy(i => i.Name);
        }

        /// <summary>
        /// Makes a request to increment the day and update all inventory items accordingly
        /// </summary>
        /// <returns><see cref="IEnumerable{InventoryItem}"/> representing current inventory items</returns>
        [HttpPost("PostIncrementDay")]
        public async Task<IEnumerable<InventoryItem>> PostIncrementDay()
        {
            _InventoryIncrementDayService.IncrementDayForAllItems();
            return await GetInventoryItemsAsync();
        }
    }
}
