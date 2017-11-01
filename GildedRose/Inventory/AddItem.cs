using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace GildedRose.Inventory
{
    public class AddItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public int sellin { get; set; }
        public int quality { get; set; }
    }
}
