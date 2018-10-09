using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using guilded.rose.api.Domain.Business.Interfaces;
using guilded.rose.api.Domain.DataAccess;
using guilded.rose.api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace guilded.rose.api.Domain.Business
{
    public class CategoryBL : ICategoryBL
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryBL(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<List<Category>> GetCategories()
        {
            return await _categoryRepository.Get();
        }
    }
}