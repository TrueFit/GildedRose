using Autofac;
using GildedRose.Contracts;
using GildedRose.Store.DependencyManagement;

namespace GildedRose.Managers.DependencyManagement
{
    public class ManagerModule : Module
    {
        public string ConnectionString { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new StoreModule()
            {
                ConnectionString = this.ConnectionString,
            });

            builder.RegisterType<ItemManager>().As<IItemManager>().InstancePerLifetimeScope();
        }
    }
}
