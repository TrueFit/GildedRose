using Autofac;
using GildedRose.Core.Contracts;
using GildedRose.Store.Base;
using GildedRose.Store.Contracts;
using GildedRose.Store.Wrappers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace GildedRose.Store.DependencyManagement
{
    public class StoreModule : Module
    {
        public string ConnectionString { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
            {
                return new SqlConnection(this.ConnectionString);
            }).As<System.Data.IDbConnection>().InstancePerDependency();

            //builder.Register<DataStore>(x =>
            //{
            //    var userContext = x.Resolve<Context>();
            //    var connectionFactory = x.Resolve<Func<System.Data.IDbConnection>>();
            //    return new DataStore(userContext.UserId, connectionFactory);
            //}).As<DataStore>().As<IDataStore>().InstancePerDependency();
            builder.Register<DataStore>(x =>
            {
                var connectionFactory = x.Resolve<Func<System.Data.IDbConnection>>();
                return new DataStore(connectionFactory);
            }).As<DataStore>().As<IDataStore>().InstancePerDependency();

            builder.RegisterType<DapperSqlBuilder>().As<ISqlBuilder>().InstancePerDependency();

            //builder.Register<BulkDataStore>(x =>
            //{
            //    /var userContext = x.Resolve<Context>();
            //    var connectionFactory = x.Resolve<Func<System.Data.IDbConnection>>();
            //    return new BulkDataStore(userContext.UserId, connectionFactory);
            //}).As<BulkDataStore>().InstancePerDependency();
            builder.Register<BulkDataStore>(x =>
            {
                var connectionFactory = x.Resolve<Func<System.Data.IDbConnection>>();
                return new BulkDataStore(connectionFactory);
            }).As<BulkDataStore>().InstancePerDependency();
        }
    }
}
