using System.Collections.Generic;
using System.Threading.Tasks;
using guilded.rose.api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace guilded.rose.api.Domain.DataAccess
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly GuildedRoseContext _context;
        public CategoryRepository(GuildedRoseContext context) {
            _context = context;
        }
        public async Task<List<Category>> Get() => await _context.Categories.ToListAsync();

        public async Task<Category> GetByName(string name) => await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
    }
}