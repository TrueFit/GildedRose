using Autofac;
using GildedRose.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Managers.DependencyManagement
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ItemManager>().As<IItemManager>().InstancePerLifetimeScope();
        }
    }
}
