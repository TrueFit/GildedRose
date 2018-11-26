using Autofac;
using GildedRose.Membership.Crypto;
using GildedRose.Core.Contracts;
using GildedRose.Configuration;

namespace GildedRose.Membership.DependencyManagement
{
    public class MembershipModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationStore>().As<IConfigurationStore>().InstancePerLifetimeScope();
            builder.RegisterType<PasswordHasher>().As<PasswordHasher>().InstancePerDependency();
        }
    }
}
