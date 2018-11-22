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
using Autofac.Extensions.DependencyInjection;

namespace GildedRose.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment env;

        public Startup(
            IHostingEnvironment env)
        {
            this.env = env;

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Machine);
            var appParentDirectory = new DirectoryInfo(this.env.ContentRootPath).Parent.FullName;

            // Lets not take their word for it and use the environment variable and our own convention
            // var environmentName = this.env.EnvironmentName ?? "Dev";
            var environmentName = environment ?? "Dev";
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            // the 3 lines below fix the issue mentioned here: https://github.com/aspnet/Home/issues/3132
            // without then, the build will fail.
            //var manager = new ApplicationPartManager();
            //manager.ApplicationParts.Add(new AssemblyPart(typeof(Startup).Assembly));
            //services.AddSingleton(manager);
            services.AddSingleton(this.Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Item Service", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
            });

            services
                .AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1)
                .AddFluentValidation(x => x.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

            var connectionString = this.Configuration.GetConnectionString("GildedRose");
            var containerBuilder = ServiceConfiguration.Register(this.AddWebServices, connectionString);
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
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

        private void AddWebServices(ContainerBuilder builder)
        {
        }
    }
}
