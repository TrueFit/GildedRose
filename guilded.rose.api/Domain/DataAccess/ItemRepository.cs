using System.Collections.Generic;
using System.Threading.Tasks;
using guilded.rose.api.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace guilded.rose.api.Domain.DataAccess
{
    public class ItemRepository : IRepository<Item>
    {
        private readonly GuildedRoseContext _context;
        public ItemRepository(GuildedRoseContext context)
        {
            _context = context;
        }
        public async Task<List<Item>> Get() => await _context.Items.Include(cat => cat.Category).ToListAsync();

        public async Task<Item> GetByName(string name) => await _context.Items.Include(cat => cat.Category).FirstOrDefaultAsync(i => i.Name == name);
    }
}