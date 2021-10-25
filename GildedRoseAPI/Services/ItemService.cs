using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GildedRoseAPI.Data;
using GildedRoseAPI.Models;
using GildedRoseAPI.Requests;
using GildedRoseAPI.Responses;

namespace GildedRoseAPI.Services
{
    public class ItemService
    {
        private static readonly int MAX_QUALITY = 50;

        private readonly GildedRoseDBContext _db;

        public ItemService(GildedRoseDBContext db)
        {
            _db = db;
        }

        public GenericResponse ResetItems()
        {
            GenericResponse response = new GenericResponse();

            //Remove existing items (todo: surely there is a better method?)
            _db.Items.RemoveRange(_db.Items.ToList());

            //Add stock data
            List<Item> items = new List<Item>();
            items.Add(new Item() { Name = "Sword", Category = "Weapon", SellIn = 30, Quality = 50 });
            items.Add(new Item() { Name = "Axe", Category = "Weapon", SellIn = 40, Quality = 50 });
            items.Add(new Item() { Name = "Halberd", Category = "Weapon", SellIn = 60, Quality = 40 });
            items.Add(new Item() { Name = "Aged Brie", Category = "Food", SellIn = 50, Quality = 10 });
            items.Add(new Item() { Name = "Aged Milk", Category = "Food", SellIn = 20, Quality = 20 });
            items.Add(new Item() { Name = "Mutton", Category = "Food", SellIn = 10, Quality = 10 });
            items.Add(new Item() { Name = "Hand of Ragnaros", Category = "Sulfuras", SellIn = 80, Quality = 80 });
            items.Add(new Item() { Name = "I am Murloc", Category = "Backstage Passes", SellIn = 20, Quality = 10 });
            items.Add(new Item() { Name = "Raging Ogre", Category = "Backstage Passes", SellIn = 10, Quality = 10 });
            items.Add(new Item() { Name = "Giant Slayer", Category = "Conjured", SellIn = 15, Quality = 50 });
            items.Add(new Item() { Name = "Storm Hammer", Category = "Conjured", SellIn = 20, Quality = 50 });
            items.Add(new Item() { Name = "Belt of Giant Strength", Category = "Conjured", SellIn = 20, Quality = 40 });
            items.Add(new Item() { Name = "Cheese", Category = "Food", SellIn = 5, Quality = 5 });
            items.Add(new Item() { Name = "Potion of Healing", Category = "Potion", SellIn = 10, Quality = 10 });
            items.Add(new Item() { Name = "Bag of Holding", Category = "Misc", SellIn = 10, Quality = 50 });
            items.Add(new Item() { Name = "TAFKAL80ETC Concert", Category = "Backstage Passes", SellIn = 15, Quality = 20 });
            items.Add(new Item() { Name = "Elixir of the Mongoose", Category = "Potion", SellIn = 5, Quality = 7 });
            items.Add(new Item() { Name = "+5 Dexterity Vest", Category = "Armor", SellIn = 10, Quality = 20 });
            items.Add(new Item() { Name = "Full Plate Mail", Category = "Armor", SellIn = 50, Quality = 50 });
            items.Add(new Item() { Name = "Wooden Shield", Category = "Armor", SellIn = 10, Quality = 30 });
            _db.AddRange(items);

            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = $"Error resetting items in database: {ex.Message}";
                return response;
            }

            response.success = true;
            response.message = "Reset all items.";
            return response;
        }

        public ItemListResponse GetItems()
        {
            ItemListResponse response = new ItemListResponse();
            response.items = _db.Items.ToList();
            response.success = true;
            response.message = "Retrieved all items.";
            return response;
        }

        public ItemListResponse GetItemsToTrash()
        {
            ItemListResponse response = new ItemListResponse();
            response.items = _db.Items.Where(i => i.Quality <= 0).ToList();
            response.success = true;
            response.message = "Retrieved all items to be trashed.";
            return response;
        }

        public ItemResponse GetItem(ItemRequest request)
        {
            ItemResponse response = new ItemResponse();
            Item item = _db.Items.Where(i => i.Name == request.name).FirstOrDefault();

            if (item == null)
            {
                response.success = false;
                response.message = $"Unable to find item with name: {request.name}.";
                return response;
            }

            response.item = item;
            response.success = true;
            response.message = $"Retrieved items with name: {request.name}.";
            return response;
        }

        /*
        public GenericResponse TrashItem(ItemRequest request)
        {
            GenericResponse response = new GenericResponse();
            Item item = _db.Items.Where(i => i.Name == request.name).FirstOrDefault();

            if (item == null)
            {
                response.success = false;
                response.message = $"Unable to find item with name: {request.name}.";
                return response;
            }

            try
            {
                _db.Items.Remove(item);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = $"Error deleting item from database: {ex.Message}";
                return response;
            }
            
            response.success = true;
            response.message = $"Trashed item with name: {request.name}.";
            return null;
        }
        */

        public GenericResponse ProgressToNextDay()
        {
            GenericResponse response = new GenericResponse();

            List<Item> items = _db.Items.ToList();
            items.ForEach(item => 
            {
                ProcessItem(item);
            });

            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = $"Error updating items in database: {ex.Message}";
                return response;
            }

            response.success = true;
            response.message = "One day has passed.";
            return response;
        }

        private void ProcessItem(Item item)
        {
            //Decrement SellIn, but keep from underflow.
            item.SellIn = Math.Max(item.SellIn - 1, Int32.MinValue);

            if (item.Category == "Sulfuras")
            {
                return;
            }

            //We are assuming a SellIn value of 0 isn't expired yet, but is the last day in which it can be sold.
            bool expired = item.SellIn < 0; 
            int delta = 0;

            if (item.Category == "Backstage Passes")
            {
                if (expired)
                {
                    item.Quality = 0;
                }
                else
                {
                    if (item.SellIn < 5)
                    {
                        delta = 3;
                    }
                    else if (item.SellIn < 10)
                    {
                        delta = 2;
                    }
                    else
                    {
                        delta = 1;
                    }
                }
            }
            else
            {
                //I'm assuming the "Aged Brie" rules are really only supposed to apply to items in the "Food" category.
                //If expired and/or conjured agred brie is supposed to improve twice as fast, I'd have to rewrite this.
                //Delete the if/else part, then change delta = 1; to delta = (item.Name == "Aged Brie") ? 1 : -1;
                if (item.Name == "Aged Brie")
                {
                    delta = 1;
                }
                else
                {
                    delta = -1;

                    if (expired)
                    {
                        delta *= 2;
                    }
                    if (item.Category == "Conjured")
                    {
                        delta *= 2;
                    }
                }
            }

            //Alter Quality
            item.Quality = Math.Clamp(item.Quality + delta, 0, MAX_QUALITY);
        }
    }
}
