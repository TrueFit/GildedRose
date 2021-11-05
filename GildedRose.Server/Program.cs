using GildedRose.Server.Logic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace GildedRose.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            InventoryLogic.Instance.Initialize();

            CreateHostBuilder(args).Build().Run();
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            InventoryLogic.Instance.Save();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
        }
    }
}
