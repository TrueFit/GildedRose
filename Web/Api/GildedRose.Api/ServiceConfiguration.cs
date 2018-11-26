using Autofac;
using GildedRose.Api.Validators;
using GildedRose.Configuration;
using GildedRose.Core.Contracts;
using GildedRose.Logic.Repo;
using GildedRose.Membership;
using GildedRose.Store.DependencyManagement;
using Microsoft.Extensions.Configuration;
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
            var builder = new ContainerBuilder();
            //builder.RegisterType<ConfigurationStore>().As<IConfigurationStore>().InstancePerLifetimeScope();
            builder.RegisterType<IdentityHelper>().As<IdentityHelper>().InstancePerLifetimeScope();
            builder.RegisterType<CreateAccountModel_Validator>().As<CreateAccountModel_Validator>().InstancePerLifetimeScope();
            builder.RegisterType<LoginModel_Validator>().As<LoginModel_Validator>().InstancePerLifetimeScope();

            builder
                .Register<ILogger>((c, p) =>
                {
                    return new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                .CreateLogger();
                }).SingleInstance();

            //Register modules
            builder.RegisterModule(new Membership.DependencyManagement.MembershipModule());
            builder.RegisterModule(new Logic.DependencyManagement.RepoModule()
            {
                ConnectionString = connectionString,
            });

            additionalRegistration(builder);

            return builder;
        }
    }
}