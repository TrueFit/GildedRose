using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GildedRose.Domain;
using GildedRose.Domain.Models;
using GildedRose.Models;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace GildedRose
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<InventoryContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("InventoryConnection")));

            services.AddAutoMapper();
            services.AddCors();
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Inventory API", Version = "v1" });
            });

            services.AddTransient<IDatabase<Models.Inventory>, InventoryData>();
            services.AddTransient<IDatabase<Category>, CategoryData>();
            services.AddTransient<IDatabase<InventoryItem>, ItemData>();

            services.AddTransient<IDomain<InventoryValue>, InventoryDomain>();
            services.AddTransient<IDomain<CategoryValue>, CategoryDomain>();
            services.AddTransient<IDomain<InventoryItemValue>, InventoryItemDomain>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory API V1");
            });

            app.UseCors(builder =>
                         builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials()
            );

            app.UseMvc();
        }
    }
}
