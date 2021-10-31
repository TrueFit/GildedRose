using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccessLibrary;
using GildedRose.Helpers;

namespace GildedRose.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryContext _context;

        public InventoryController(InventoryContext context)
        {
            _context = context;
        }

        //Re-populate DB with default example data
        [HttpPost("RepopulateDataToDefault")]
        public async Task<ActionResult> RepopulateDataToDefault()
        {
            await DataCreateAndDrop.CreateExampleData();
            return Ok();
        }

        //Get All Items
        [HttpGet("GetAllItems")]
        public async Task<ActionResult<List<Item>>> GetAllItems()
        {
            var items = await _context.Items.AsNoTracking().Where(x => true).ToListAsync();
            if (items == null) return NotFound();
            return items;
        }

        //Get a single item
        [HttpGet("GetAnItem/{itemId}")]
        public async Task<ActionResult<Item>> GetAnItem(int itemId)
        {
            var item = await _context.Items.AsNoTracking().FirstOrDefaultAsync(x => x.ItemId == itemId);
            if (item == null) return NotFound();
            return item;
        }

        //Add a single item
        [HttpPost("AddAnItem")]
        public async Task<ActionResult<Item>> AddAnItem([FromBody] Item item)
        {
            if (item.CategoryId <= 0) return BadRequest($"error: Category ID {item.CategoryId} is not valid");
            if (item.SellIn < 0) return BadRequest("error: SellIn value must be greater than or equal to 0");
            if (string.IsNullOrWhiteSpace(item.ItemName)) return BadRequest($"error: ItemName field must contain a value");

            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        //Delete a single item
        [HttpDelete("DeleteAnItem/{itemId}")]
        public async Task<ActionResult> DeleteAnItem(int itemId)
        {
            var item = await _context.Items.AsNoTracking().FirstOrDefaultAsync(x => x.ItemId == itemId);
            if (item == null) return NotFound();
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return Ok();
        }

        //Delete multiple items
        [HttpDelete("DeleteMultipleItems/")]
        public async Task<ActionResult> DeleteAnItem([FromBody] int[] itemIds)
        {
            var items = await _context.Items.AsNoTracking().Where(x => itemIds.Contains(x.ItemId)).ToListAsync();
            if (items == null) return NotFound();
            _context.Items.RemoveRange(items);
            await _context.SaveChangesAsync();
            return Ok();
        }

        //Get all items eligible to be trashed
        [HttpGet("GetAllItemsForTrash")]
        public async Task<ActionResult<List<Item>>> GetAllItemsToTrash()
        {
            var items = await _context.Items.AsNoTracking().Where(x => x.Quality == 0).ToListAsync();
            if (items == null) return new List<Item>();
            return items;
        }

        //Get All Categories
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            var categories = await _context.Categories.AsNoTracking().Where(x => true).ToListAsync();
            if (categories == null) return NotFound();
            return categories;
        }

    }
}
