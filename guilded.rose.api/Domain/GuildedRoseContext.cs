using System;
using guilded.rose.api.Domain.Models;
using guilded.rose.api.Domain.Infrastructure.DataLoaders;
using guilded.rose.api.Domain.Infrastructure.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace guilded.rose.api.Domain
{
    public class GuildedRoseContext : DbContext
    {
        public GuildedRoseContext(DbContextOptions<GuildedRoseContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasKey(key => key.Id);
            modelBuilder.Entity<Category>().Property<int>(e => e.Id).UseSqlServerIdentityColumn();

            modelBuilder.Entity<Item>().HasKey(key => key.Id);
            modelBuilder.Entity<Item>().Property(i => i.Id).UseSqlServerIdentityColumn();

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Category);

            modelBuilder.Entity<Item>().Property(i => i.DateCreated).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Item>().Property(i => i.DateCreated).ValueGeneratedOnAdd();

        }


    }
}