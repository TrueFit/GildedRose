using Autofac;
using GildedRose.Configuration;
using GildedRose.Core.Contracts;
using GildedRose.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRose.Api
{
    public class ServiceConfiguration
    {
        public static ContainerBuilder Register(Action<ContainerBuilder> additionalRegistration, string connectionString)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ConfigurationStore>().As<IConfigurationStore>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<Context>().AsSelf().InstancePerLifetimeScope();
            containerBuilder.RegisterModule(new Managers.DependencyManagement.ServiceModule());
            containerBuilder.RegisterModule(new StoreModule()
            {
                ConnectionString = connectionString,
            });

            additionalRegistration(containerBuilder);

            return containerBuilder;
        }

    }
}
