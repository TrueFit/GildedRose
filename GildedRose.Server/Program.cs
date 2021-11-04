using GildedRose.Server.Logic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GildedRose.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InventoryLogic.Instance.Initialize();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
