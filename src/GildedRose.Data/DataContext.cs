using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GildedRose.Entities.Inventory;
using GildedRose.Entities.Inventory.Aging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GildedRose.Data
{
    public interface IDataContext : IDisposable
    {
        ChangeTracker ChangeTracker { get; }
        DbSet<Inventory> Inventory { get; set; }
        DbSet<InventoryHistory> InventoryHistory { get; set; }
        IEnumerable<BaseAgingRule> AgingRules { get; }
        Task<int> SaveChangesAsync();
    }

    /// <summary>
    /// Entity Framework Core DbContext class for Gilded Rose data access
    /// </summary>
    public class DataContext : DbContext, IDataContext
    {
        #region DbSets

        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<InventoryHistory> InventoryHistory { get; set; }

        #endregion

        public new ChangeTracker ChangeTracker => base.ChangeTracker;

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        #region Aging Rules

        public IEnumerable<BaseAgingRule> AgingRules => GetAgingRules();

        #endregion

        #region OnConfiguring Method

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DataContextHelper.GetDatabasePath()}");
        }

        #endregion

        #region Private Helper Methods

        private IEnumerable<BaseAgingRule> GetAgingRules()
        {
            return DataContextHelper.LoadAgingRules();
        }

        #endregion
    }
}
