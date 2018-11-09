using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GildedRose.Api
{
    public class Program
    {
        private const string RootLifetimeTag = "MyIsolatedRoot";

        public static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<MessageHandler>().As<IHandler>();
            var container = containerBuilder.Build();

            using (var scope = container.BeginLifetimeScope(RootLifetimeTag, b =>
            {
                b.Populate(serviceCollection, RootLifetimeTag);
            }))
            {
                // This service provider will have access to global singletons
                // and registrations but the "singletons" for things registered
                // in the service collection will be "rooted" under this
                // child scope, unavailable to the rest of the application.
                //
                // Obviously it's not super helpful being in this using block,
                // so likely you'll create the scope at app startup, keep it
                // around during the app lifetime, and dispose of it manually
                // yourself during app shutdown.
                var serviceProvider = new AutofacServiceProvider(scope);



                CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((context, options) =>
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Machine);
                    var appParentDirectory = new DirectoryInfo(context.HostingEnvironment.ContentRootPath).Parent.FullName;
                    var environmentName = context.HostingEnvironment.EnvironmentName ?? "Dev";
                    options.SetBasePath(Directory.GetCurrentDirectory())

                    .AddCommandLine(args)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
                });
    }
}
