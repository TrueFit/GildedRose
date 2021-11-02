using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GildedRose_Blazor.Server.Helpers;
using GildedRose_Blazor.Shared;

namespace GildedRose_Blazor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryContext _context;
        private readonly List<Category> _categories;

        public InventoryController(InventoryContext context)
        {
            _context = context;
            _categories = _context.Categories.ToList();

        }

        //Re-populate DB with default example data
        [HttpGet("RepopulateDataToDefault")]
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
            if (!items.Any()) return NotFound();
            //populate category name field
            foreach (Item item in items)
            {
                item.CategoryName = _categories.FirstOrDefault(x => x.CategoryId == item.CategoryId)?.CategoryName;
            }
            //order by category name
            items = items.OrderBy(x => x.CategoryName).ToList();
            return items;
        }

        //Get a single item by id
        [HttpGet("GetAnItem")]
        public async Task<ActionResult<Item>> GetAnItem(int itemId)
        {
            var item = await _context.Items.AsNoTracking().FirstOrDefaultAsync(x => x.ItemId == itemId);
            if (item == null) return NotFound();
            //populate category name field
            item.CategoryName = _categories.FirstOrDefault(x => x.CategoryId == item.CategoryId)?.CategoryName;
            return item;
        }

        //Get a single item by name
        [HttpGet("GetAnItemByName")]
        public async Task<ActionResult<Item>> GetAnItemByName(string itemName)
        {
            var item = await _context.Items.AsNoTracking().FirstOrDefaultAsync(x => x.ItemName.ToLower() == itemName.ToLower());
            if (item == null) return NotFound();
            //populate category name field
            item.CategoryName = _categories.FirstOrDefault(x => x.CategoryId == item.CategoryId)?.CategoryName;
            return item;
        }

        //Get a multiple items by partial name
        [HttpGet("GetMultipleItemsByPartialName")]
        public async Task<ActionResult<List<Item>>> GetMultipleItemsByPartialName(string itemName)
        {
            var items = await _context.Items.AsNoTracking().Where(x => x.ItemName.ToLower().Contains(itemName.ToLower())).ToListAsync();
            if (items == null) return NotFound();
            //populate category name field
            foreach (Item item in items)
            {
                item.CategoryName = _categories.FirstOrDefault(x => x.CategoryId == item.CategoryId)?.CategoryName;
            }
            //order by Item Name
            items = items.OrderBy(x => x.ItemName).ToList();
            return items;
        }

        //Add a single item
        [HttpPost("AddAnItem")]
        public async Task<ActionResult<Item>> AddAnItem([FromBody] Item item)
        {
            //qualify all fields are valid
            if (item.CategoryId <= 0) return BadRequest($"error: Category ID {item.CategoryId} is <=0 or null");
            if (item.Quality < 0) return BadRequest("error: Quality must be >= 0");
            if (item.SellIn < 0) return BadRequest("error: SellIn value must be >= 0");
            if (string.IsNullOrWhiteSpace(item.ItemName)) return BadRequest("error: ItemName field must contain a value");
            if (!_context.Categories.Any(x=>x.CategoryId == item.CategoryId)) return BadRequest($"error: Category ID {item.CategoryId} does not exist");
            if (_context.Items.Any(x=>x.ItemName.ToLower() == item.ItemName.ToLower())) return BadRequest($"error: Item {item.ItemName} already exists");
            //populate category name field
            item.CategoryName = _categories.FirstOrDefault(x => x.CategoryId == item.CategoryId)?.CategoryName;
            //save changes
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        //Delete a single item
        [HttpDelete("DeleteAnItem/")]
        public async Task<ActionResult> DeleteAnItem([FromBody] int itemId)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.ItemId == itemId);
            if (item == null) return NotFound();
            //save changes
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return Ok();
        }

        //Delete multiple items
        [HttpDelete("DeleteMultipleItems/")]
        public async Task<ActionResult> DeleteMultipleItems([FromBody] int[] itemIds)
        {
            var items = await _context.Items.Where(x => itemIds.Contains(x.ItemId)).ToListAsync();
            if (items == null) return NotFound();
            if (items.Count != itemIds.Length) return BadRequest("error: Not able to match all itemId's to an item record");
            //save changes
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
            //populate category name field
            foreach (Item item in items)
            {
                item.CategoryName = _categories.FirstOrDefault(x => x.CategoryId == item.CategoryId)?.CategoryName;
            }
            items = items.OrderBy(x => x.CategoryName).ToList();
            return items;
        }

        //Delete all items eligible to be trashed
        [HttpGet("DeleteAllItemsForTrash")]
        public async Task<ActionResult> DeleteAllItemsForTrash()
        {
            var items = await _context.Items.Where(x => x.Quality == 0).ToListAsync();
            if (items == null) return NotFound();
            //save changes
            _context.Items.RemoveRange(items);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
