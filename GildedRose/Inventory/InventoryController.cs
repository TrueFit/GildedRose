using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;

namespace GildedRose.Inventory
{
    public class InventoryController : Controller
    {
        public readonly MongoContext mongoContext = new MongoContext();

        // GET: Inventory
        public ActionResult Index()
        {
            var inventory = mongoContext.Inventory.AsQueryable<Product>().ToList();
            return View(inventory);
        }

        // GET: Inventory/Details/5
        public ActionResult Details(string id)
        {
            //Should query a item server side
            var inventory = mongoContext.Inventory.AsQueryable<Product>().ToList();

            Product product = (from p in inventory
                              where p.Id == id
                              select p).Single<Product>();
              
            return View(product);
        }

        // GET: Inventory/Create
        public ActionResult AddItem()
        {
            return View();
        }

        // POST: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(AddItem addItem)
        {
            try
            {
                // TODO: Add insert logic here
                var product = new Product(addItem);
                mongoContext.Inventory.InsertOne(product);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Inventory/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult CloseDay()
        {
            CloseDay closeDay = new Inventory.CloseDay();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ShowTrash()
        {
            var inventory = mongoContext.Inventory.AsQueryable<Product>().ToList().Where(p => p.quality == 0);
            return View(inventory);
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Inventory/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Inventory/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}