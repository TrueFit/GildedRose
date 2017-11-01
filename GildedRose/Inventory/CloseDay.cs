using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace GildedRose.Inventory
{
    public class CloseDay
    {
        public MongoContext mongoContext = new MongoContext();
        public List<Product> inventory;

        public CloseDay()
        {
            //Get the inventory
            inventory = mongoContext.Inventory.AsQueryable<Product>().ToList();

            ApplyRules();
        }

        public void ApplyRules()
        {

            foreach (Product p in inventory)
            {
                DecrementSellin(p);
                UpdateQuality(p);
            }

            //Good update document here instead of fields

        }

        private void UpdateQuality(Product p)
        {
          
            if(p.category == "Backstage Passes" && p.sellin >= -1)
            {
                ProcessBackstagePassess(p);
            }
            else if (p.name == "Aged Brie") //Hard coded do to time 
            {
                if(p.quality < 50)
                {
                    p.quality++;
                }
            }
            else if  (p.quality != 0 && p.category != "Sulfuras")
            {
                if (p.category == "Conjured")
                {
                    p.quality = (p.quality == 1) ? 0 : p.quality -2;
                }
                else
                {
                    p.quality = (p.quality == 0) ? 0 : p.quality - 1;
                }
            }

            var filter = Builders<Product>.Filter.Eq("Id", new ObjectId(p.Id));
            var update = Builders<Product>.Update
                .Set("quality", p.quality);

            var updateResult = mongoContext.Inventory.UpdateOne(filter, update);

        }

        private void ProcessBackstagePassess(Product p)
        {
            const int maxQuality = 50;

            if (p.sellin == -1)
            {
                p.quality = 0;
            }
            else if (p.sellin <= 5 && p.sellin > -1)
            {
                p.quality = (p.quality <= (maxQuality -3)) ? p.quality + 3 : maxQuality;
            }
            else if (p.sellin <= 10 && p.sellin > 5)
            {
                p.quality = (p.quality <= (maxQuality - 2)) ? p.quality + 2 : maxQuality;
            }
            else
            {
                p.quality = (p.quality <= (maxQuality - 1)) ? p.quality + 1 : maxQuality;
            }
        }

   

        public void DecrementSellin(Product product)
        {
            //Implement Legendary as field
            if (product.category != "Sulfuras")
            {
                product.sellin--;
            }

            var filter = Builders<Product>.Filter.Eq("Id", new ObjectId(product.Id));
            var update = Builders<Product>.Update
                .Set("sellin", product.sellin);

            var updateResult = mongoContext.Inventory.UpdateOne(filter, update);

        }
    }


}