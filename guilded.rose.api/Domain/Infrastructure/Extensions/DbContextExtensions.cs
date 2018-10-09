using System;
using System.Collections.Generic;
using System.Linq;
using guilded.rose.api.Domain;
using guilded.rose.api.Domain.Models;
using guilded.rose.api.Domain.Infrastructure.DataLoaders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace guilded.rose.api.Domain.Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        public static void EnsureSeeded(this GuildedRoseContext context)
        {
            var imported = new CsvDataLoader("guilded.rose.api.Domain.Data.Inventory.csv").Build();

            if (!context.Categories.Any())
            {
                var categories = imported.Select(import => import.Category).Distinct().Select(cat => new Category { Name = cat });

                context.AddRange(categories);
                context.SaveChanges();
            }

            if (!context.Items.Any())
            {
                var items = imported.Select(import => new Item
                {
                    Name = import.Name,
                    Category = context.Categories.First(cat => cat.Name == import.Category),
                    CategoryId = context.Categories.First(cat => cat.Name == import.Category).Id,
                    SellIn = import.SellIn,
                    Quality = import.Quality,
                    DateCreated = DateTime.Now
                });

                context.AddRange(items);
                context.SaveChanges();

            }

        }
    }
}