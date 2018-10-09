using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using guilded.rose.api.Domain;
using guilded.rose.api.Domain.Business;
using guilded.rose.api.Domain.Business.Interfaces;
using guilded.rose.api.Domain.DataAccess;
using guilded.rose.api.Domain.Infrastructure.Extensions;
using guilded.rose.api.Domain.Models;
using guilded.rose.api.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace guilded.rose.api
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
            // TODO: move to config
            var connection = @"Server=(local);Database=GuildedRose;Trusted_Connection=True;";

            services.AddDbContext<GuildedRoseContext>(opt => opt.UseSqlServer(connection));

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepository<Item>, ItemRepository>();

            services.AddScoped<ICategoryBL, CategoryBL>();
            services.AddScoped<IItemBL, ItemBL>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.ConfigureCustomExceptionMiddleware();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().Build());

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<GuildedRoseContext>().EnsureSeeded();
            }


            app.UseHttpsRedirection();
            app.UseMvc();

        }
    }
}
