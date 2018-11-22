using Autofac;
using GildedRose.Configuration;
using GildedRose.Contracts;
using GildedRose.Core.Contracts;
using GildedRose.Managers;
using GildedRose.Store;
using GildedRose.Store.DependencyManagement;
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

            //containerBuilder.RegisterType<Context>().AsSelf().InstancePerLifetimeScope();

            //Register Managers
            containerBuilder.RegisterModule(new Managers.DependencyManagement.ServiceModule());

            // Register Store Module
            containerBuilder.RegisterModule(new StoreModule()
            {
                ConnectionString = connectionString,
            });

            additionalRegistration(containerBuilder);

            return containerBuilder;
        }

    }
}
