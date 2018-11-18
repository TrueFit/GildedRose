using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using GildedRose.Models;

namespace GildedRose.Persistence.Context
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>().ToTable("Inventory"); 
            modelBuilder.Entity<InventoryItem>().ToTable("InventoryItem");
            modelBuilder.Entity<Quality>().ToTable("ItemQuality");
            modelBuilder.Entity<Degradation>().ToTable("Degradation");
            modelBuilder.Entity<Category>().ToTable("Category");
        }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Quality> QualityItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Degradation> DegradationItems { get; set; }
    }
}

