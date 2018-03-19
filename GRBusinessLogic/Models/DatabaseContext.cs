using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GR.BusinessLogic.Models{
     public class DatabaseContext : DbContext{
         public DbSet<Item> Items { get; set; }

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            
            // This is using a Docker MySql database container 
            optionsBuilder.UseMySQL("server=localhost;port=6606;database=GildedRose;user=root;password=mypassword");
         }
     }
}