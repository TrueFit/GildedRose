using System;
using System.Threading.Tasks;
using GuildedRose.Inventory.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GuildedRose.Inventory
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var inventoryContext = services.GetRequiredService<InventoryDbContext>();

                    // For testing purposes delete the database each time the program is started
                    // Remove this line if you would like the data to be persisted across runs.
                    // TODO: For production environment this should be removed and potentially
                    // replacing SQLite database with whatever makes sense in the customer environment.
                    await inventoryContext.Database.EnsureDeletedAsync();

                    await inventoryContext.Database.EnsureCreatedAsync();
                    await InventoryContextSeed.SeedAsync(inventoryContext);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred seeding the database");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
