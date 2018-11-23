using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GildedRose.Membership.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using GildedRose.Api.Extensions;
using GildedRose.Membership.Data;
using Microsoft.EntityFrameworkCore;
using GildedRose.Membership.Filters;
using System.Security.Claims;
using System.Collections.Generic;

namespace GildedRose.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment env;

        public Startup(
            IHostingEnvironment env,
            IConfiguration config)
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
            // TODO: ENABLE OR REMOVE THESE LINES!!!
            // the 3 lines below fix the issue mentioned here: https://github.com/aspnet/Home/issues/3132
            // without then, the build will fail.
            //var manager = new ApplicationPartManager();
            //manager.ApplicationParts.Add(new AssemblyPart(typeof(Startup).Assembly));
            //services.AddSingleton(manager);
            services.AddSingleton(this.Configuration);

            var jwt = this.Configuration.GetSection("Jwt").Get<Jwt>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                };
            });

            services
                   .AddMvc(o =>
                   {
                       o.Conventions.Add(new AddAuthorizeFiltersControllerConvention());
                   })
                   .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1)
                   .AddFluentValidation(x => x.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddAuthorization(o =>
            {
                o.AddPolicy("securepolicy", b =>
                {
                    b.RequireAuthenticatedUser();
                    b.AuthenticationSchemes = new List<string> { JwtBearerDefaults.AuthenticationScheme };
                });
            });

            var platformConnectionString = this.Configuration.GetConnectionString("GildedRose.Platform");
            var membershipConnectionString = this.Configuration.GetConnectionString("GildedRose.Membership");

            var containerBuilder = ServiceConfiguration.Register(this.AddWebServices, platformConnectionString);

            services.AddEntityFrameworkSqlServer().AddDbContext<UserDbContext>(options =>
            {
                options.UseSqlServer(membershipConnectionString);
            });

            services.AddSwaggerDocumentation();

            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();
            app.UseMvc();
        }

        private void AddWebServices(ContainerBuilder builder)
        {
        }
    }
}
