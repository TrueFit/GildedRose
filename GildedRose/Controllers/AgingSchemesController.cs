using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GildedRose.Models;

namespace GildedRose.Controllers
{
    public class AgingSchemesController : Controller
    {
        private GildedRoseContext db = new GildedRoseContext();

        // GET: AgingSchemes
        public ActionResult Index()
        {
            var agingSchemes = db.AgingSchemes.Include(a => a.Product);
            return View(agingSchemes.ToList());
        }

        // GET: AgingSchemes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgingScheme agingScheme = db.AgingSchemes.Find(id);
            if (agingScheme == null)
            {
                return HttpNotFound();
            }
            return View(agingScheme);
        }

        // GET: AgingSchemes/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
            return View();
        }

        // POST: AgingSchemes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AgingSchemeId,SchemeName,DefaultIncrement,MaxQuality,ScrapOnExpiration,ProductId,LastUpdated")] AgingScheme agingScheme)
        {
            if (ModelState.IsValid)
            {
                agingScheme.AgingSchemeId = Guid.NewGuid();
                db.AgingSchemes.Add(agingScheme);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", agingScheme.ProductId);
            return View(agingScheme);
        }

        // GET: AgingSchemes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgingScheme agingScheme = db.AgingSchemes.Find(id);
            if (agingScheme == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", agingScheme.ProductId);
            return View(agingScheme);
        }

        // POST: AgingSchemes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AgingSchemeId,SchemeName,DefaultIncrement,MaxQuality,ScrapOnExpiration,ProductId,LastUpdated")] AgingScheme agingScheme)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agingScheme).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", agingScheme.ProductId);
            return View(agingScheme);
        }

        // GET: AgingSchemes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgingScheme agingScheme = db.AgingSchemes.Find(id);
            if (agingScheme == null)
            {
                return HttpNotFound();
            }
            return View(agingScheme);
        }

        // POST: AgingSchemes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AgingScheme agingScheme = db.AgingSchemes.Find(id);
            db.AgingSchemes.Remove(agingScheme);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
