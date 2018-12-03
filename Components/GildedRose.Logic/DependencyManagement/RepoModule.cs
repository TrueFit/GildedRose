using Autofac;
using GildedRose.Logic.Repo;

namespace GildedRose.Logic.DependencyManagement
{
    public class RepoModule : Module
    {
        public string ConnectionString { get; set; }

        public int SQLTimeout { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Managers.DependencyManagement.ManagerModule()
            {
                ConnectionString = this.ConnectionString,
                SQLTimeout = this.SQLTimeout,
            });

            builder.RegisterType<ItemRepo>().As<ItemRepo>().InstancePerLifetimeScope();
        }
    }
}