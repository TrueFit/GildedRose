using GildedRose.Data;
using GildedRose.Data.Interfaces;
using GildedRose.Data.Repos;
using GildedRose.Services;
using GildedRose.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace WebApplication1
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
            // this is where all of the magic occurs.  By adding our database context here along with adding
            // our repositories and services, the Web API framework injects the correct dependency into the
            // controllers and repos at run time.
            services.AddDbContext<StoreContext>(options => options.UseInMemoryDatabase("GildedRose").EnableSensitiveDataLogging());
            services.AddScoped<IStoreItemRepository, StoreItemRepository>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IEndOfDayService, EndOfDayService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Gilded Rose API", Version = "v1" });
            });

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

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gilded Rose API V1");
                // serve the Swagger UI as default page at the app root
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
