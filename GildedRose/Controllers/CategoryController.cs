using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GildedRose.Persistence.Context;
using GildedRose.Models;
using GildedRose.Persistence;
using GildedRose.Persistence.Database;
using AutoMapper;
using GildedRose.Domain.Models;
using GildedRose.Domain;

namespace GildedRose.Inventory.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Categories")]
    public class CategoryController : Controller
    {
        private readonly InventoryContext _context;
        private readonly IDatabase<Category> _categories;
        private readonly CategoryDomain _categoryDomain;
        private readonly IMapper _mapper;

        public CategoryController(InventoryContext context, IMapper mapper, IDatabase<Category> categories, IDomain<CategoryValue> categoryDomain)
        {
            _context = context;
            _mapper = mapper;
            _categories = categories;
            _categoryDomain = (CategoryDomain)categoryDomain;
        }

        // GET: api/Categories
        [HttpGet]
        public IEnumerable<CategoryValue> GetCategories()
        {
            return _categoryDomain.GetCategories();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public CategoryValue GetItem(int id)
        {
            return _categoryDomain.GetCategory(id);
        }

        // POST: api/Categories
        [HttpPost]
        public void AddItem([FromBody]CategoryValue category)
        {
            _categoryDomain.AddCategory(category);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public void PutInventory([FromBody]CategoryValue category)
        {
            _categoryDomain.AddCategory(category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public void DeleteItem(int categoryId)
        {
            _categoryDomain.DeleteCategory(categoryId);
        }
    }
}
