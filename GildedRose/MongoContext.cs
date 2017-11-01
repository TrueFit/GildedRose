using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using GildedRose.Inventory;
using MongoDB.Bson;


namespace GildedRose
{
    public class MongoContext
    {
        public IMongoDatabase Database;

        public MongoContext()
        {
            //Connect to mongo
            string mongoDBConn = GildedRose.Program.Configuration["MongoDBConn"];
            string mongoDBName = GildedRose.Program.Configuration["MongoDBName"];

            var client = new MongoClient(mongoDBConn);
            Database = client.GetDatabase(mongoDBName);
        }

        public IMongoCollection<Product> Inventory
        {
            get
            {
                var products = Database.GetCollection<Product>("inventory");
                return products;
            }
        }
    }
}
