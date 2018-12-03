using Autofac;
using GildedRose.Configuration;
using GildedRose.Contracts;
using GildedRose.Core.Contracts;
using GildedRose.Store.DependencyManagement;

namespace GildedRose.Managers.DependencyManagement
{
    public class ManagerModule : Module
    {
        public string ConnectionString { get; set; }

        public int SQLTimeout { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new StoreModule()
            {
                ConnectionString = this.ConnectionString,
            });

            builder.RegisterType<ConfigurationStore>().As<IConfigurationStore>().InstancePerLifetimeScope();
            builder.RegisterType<ItemManager>().As<IItemManager>().InstancePerLifetimeScope();
        }
    }
}
