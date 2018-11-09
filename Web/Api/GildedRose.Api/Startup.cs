using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Dapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using GildedRose.Api.Exceptions;
using GildedRose.HttpClient;
using GildedRose.Managers;
using GildedRose.Contracts;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Autofac;

namespace GildedRose.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private readonly IHostingEnvironment env;
        private readonly IConfiguration config;
        private readonly ILoggerFactory loggerFactory;

        public Startup(
            IHostingEnvironment env,
            IConfiguration config,
            ILoggerFactory loggerFactory)
        {
            this.env = env;
            this.config = config;
            this.loggerFactory = loggerFactory;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            var connectionString = this.config.GetConnectionString("GildedRose");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new CriticalStartupException("Unable to load connection string");
            }


            var containerBuilder = new ContainerBuilder();

            // Once you've registered everything in the ServiceCollection, call
            // Populate to bring those registrations into Autofac. This is
            // just like a foreach over the list of things in the collection
            // to add them to Autofac.
            containerBuilder.Populate(services);
            
            // Make your Autofac registrations. Order is important!
            // If you make them BEFORE you call Populate, then the
            // registrations in the ServiceCollection will override Autofac
            // registrations; if you make them AFTER Populate, the Autofac
            // registrations will override. You can make registrations
            // before or after Populate, however you choose.
            containerBuilder.RegisterType<MessageHandler>().As<IHandler>();

            // Creating a new AutofacServiceProvider makes the container
            // available to your app using the Microsoft IServiceProvider
            // interface so you can use those abstractions rather than
            // binding directly to Autofac.
            var container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);









            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Item Service", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
            });

            services
                .AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1)
                .AddFluentValidation(x => x.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

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

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }
    }
}
