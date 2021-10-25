using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GildedRoseAPI.Models;

namespace GildedRoseAPI.Data
{
    public class GildedRoseDBContext : DbContext
    {
        public virtual DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=GildedRoseDB;Integrated Security=true");
            }
        }
    }
}
