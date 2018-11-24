using Autofac;
using GildedRose.Api.Validators;
using GildedRose.Configuration;
using GildedRose.Core.Contracts;
using GildedRose.Membership;
using GildedRose.Store.DependencyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace GildedRose.Api
{
    public class ServiceConfiguration
    {
        public static ContainerBuilder Register(
            Action<ContainerBuilder> additionalRegistration,
            IConfigurationRoot configuration,
            string connectionString)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ConfigurationStore>().As<IConfigurationStore>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<IdentityHelper>().As<IdentityHelper>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<CreateAccountModel_Validator>().As<CreateAccountModel_Validator>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<LoginModel_Validator>().As<LoginModel_Validator>().InstancePerLifetimeScope();
            containerBuilder
                .Register<Serilog.ILogger>((c, p) =>
                {
                    return new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                .CreateLogger();
                }).SingleInstance();

            //Register Managers
            containerBuilder.RegisterModule(new Managers.DependencyManagement.ManagerModule());

            // Register Store Module
            containerBuilder.RegisterModule(new StoreModule()
            {
                ConnectionString = connectionString,
            });

            //Register Membership
            containerBuilder.RegisterModule(new Membership.DependencyManagement.MembershipModule());

            additionalRegistration(containerBuilder);

            return containerBuilder;
        }
    }
}
