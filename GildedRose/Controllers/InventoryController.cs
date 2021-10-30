using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccessLibrary;

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

        [HttpGet("{GetAllItems}")]
        public async Task<ActionResult<List<Item>>> GetAllItems()
        {
            var items = await _context.Items.AsNoTracking().Where(x => true).ToListAsync();
            if (items == null) return NotFound();
            return items;
        }

        [HttpGet("{GetAnItem}/{itemId}")]
        public async Task<ActionResult<Item>> GetAnItem(int itemId)
        {
            var item = await _context.Items.AsNoTracking().FirstOrDefaultAsync(x => x.ItemId == itemId);
            if (item == null) return NotFound();
            return item;
        }

    }
}
