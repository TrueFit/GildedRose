using Autofac;
using GildedRose.Membership.Crypto;

namespace GildedRose.Membership.DependencyManagement
{
    public class MembershipModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PasswordHasher>().As<PasswordHasher>().InstancePerDependency();
        }
    }
}
