using System;
using System.Collections.Generic;
//using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GildedRose.Inventory
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Config = config;
        }

        private IConfiguration Config { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Config.GetConnectionString("InventoryDb");
            services.AddSingleton(new GR.Repositories.EF.Entities.InventoryDbFactory(connectionString));
            services.AddSingleton(new GR.Repositories.EF.Entities.ConfigDbFactory(connectionString));

            services.AddTransient<GR.Repositories.IInventoryRepository, GR.Repositories.EF.InventoryRepository>();
            services.AddTransient<GR.Repositories.IConfigRepository, GR.Repositories.EF.ConfigRepository>();
            services.AddTransient<GR.Managers.InventoryManager>();
            services.AddSingleton<GR.Managers.SimulationInventoryManager>();
            services.AddSingleton<GR.Managers.IInventoryManager, GR.Managers.SimulationInventoryManager>();
                //new GR.Managers.SimulationInventoryManager(
                //    sp.GetRequiredService<GR.Managers.InventoryManager>(),
                //    sp.GetRequiredService<GR.Repositories.IConfigRepository>()));

            services.AddTransient<InventoryApi.InventoryController>();
            services.AddTransient<InventoryApi.SimulationController>();

            services
                .AddMvc()
                .AddApplicationPart(typeof(InventoryApi.InventoryController).Assembly);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc(
                routes =>
                {
                    routes.MapRoute("api", "api/{controller}/{action}/{id?}");
                });
        }
    }
}
