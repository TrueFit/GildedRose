using GuildedRose.Inventory.InventoryItems;
using Microsoft.EntityFrameworkCore;

namespace GuildedRose.Inventory.Data
{
    /// <summary>
    /// The database context for inventory
    /// </summary>
    public class InventoryDbContext : DbContext
    {
        /// <summary>
        /// Constructor for <see cref="InventoryDbContext"/>
        /// </summary>
        /// <param name="options">Database context options</param>
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// All <see cref="InventoryItem"/> objects from the database.
        /// Note: These are not typed from <see cref="InventoryItemFactory"/>.
        /// </summary>
        public DbSet<InventoryItem> Items { get; set; }
    }
}