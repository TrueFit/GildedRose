using Autofac;
using GildedRose.Logic.Repo;

namespace GildedRose.Logic.DependencyManagement
{
    public class RepoModule : Module
    {
        public string ConnectionString { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Managers.DependencyManagement.ManagerModule()
            {
                ConnectionString = this.ConnectionString,
            });

            builder.RegisterType<ItemRepo>().As<ItemRepo>().InstancePerLifetimeScope();
        }
    }
}