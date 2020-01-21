#region Using Directives

using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GildedRose.Data;
using GildedRose.Domain.Inventory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace GildedRose.Tests
{
    /// <summary>
    /// Base test fixture class to help wire up services/dependencies
    /// </summary>
    public class BaseTestFixture
    {
        public IConfiguration Configuration;
        public IContainer Container;
        public IServiceProvider ServiceProvider;

        public BaseTestFixture()
        {
            ConfigureServices();
        }

        /// <summary>
        /// Configure application services
        /// </summary>
        private void ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddDbContext<DataContext>();

            Container = ConfigureContainer(services);

            ServiceProvider = new AutofacServiceProvider(Container);
        }

        /// <summary>
        /// Configure DI container builder
        /// </summary>
        /// <param name="services"></param>
        private IContainer ConfigureContainer(IServiceCollection services)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.development.json", optional: true)
                .Build();

            var builder = new ContainerBuilder();

            builder.Populate(services);

            builder.RegisterInstance(Configuration).As<IConfiguration>();
            builder.Register(c => Container).As<IContainer>();
            builder.RegisterBuildCallback(c => Container = c);

            builder.RegisterType<DataContext>().As<IDataContext>();
            builder.RegisterType<InventoryRepository>().As<IInventoryRepository>();
            builder.RegisterType<InventoryAgingCalculator>().AsSelf();

            return builder.Build();
        }
    }
}
