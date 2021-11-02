using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GildedRose_Blazor.Shared;

namespace GildedRose_Blazor.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly InventoryContext _context;

        public CategoryController(InventoryContext context)
        {
            _context = context;
        }

        //Get All Categories
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            var categories = await _context.Categories.AsNoTracking().Where(x => true).ToListAsync();
            return categories == null ? NotFound() : categories;
        }
    }
}
