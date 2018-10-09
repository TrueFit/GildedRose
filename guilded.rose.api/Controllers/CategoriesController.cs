using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using guilded.rose.api.Domain;
using guilded.rose.api.Domain.Business.Interfaces;
using guilded.rose.api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace guilded.rose.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryBL _categoryBL;
        public CategoriesController(ICategoryBL categoryBL)
        {
            _categoryBL = categoryBL;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            var categories = await _categoryBL.GetCategories();
            return Ok(categories);
        }
    }
}