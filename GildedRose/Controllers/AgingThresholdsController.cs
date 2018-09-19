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
    public class AgingThresholdsController : Controller
    {
        private GildedRoseContext db = new GildedRoseContext();

        // GET: AgingThresholds
        public ActionResult Index()
        {
            var agingThresholds = db.AgingThresholds.Include(a => a.AgingScheme);
            return View(agingThresholds.ToList());
        }

        // GET: AgingThresholds/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgingThreshold agingThreshold = db.AgingThresholds.Find(id);
            if (agingThreshold == null)
            {
                return HttpNotFound();
            }
            return View(agingThreshold);
        }

        // GET: AgingThresholds/Create
        public ActionResult Create()
        {
            ViewBag.AgingSchemeId = new SelectList(db.AgingSchemes, "AgingSchemeId", "SchemeName");
            return View();
        }

        // POST: AgingThresholds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AgingThresholdId,AgingSchemeId,ThresholdName,DaysPrior,IncrementRate,LastUpdated")] AgingThreshold agingThreshold)
        {
            if (ModelState.IsValid)
            {
                agingThreshold.AgingThresholdId = Guid.NewGuid();
                db.AgingThresholds.Add(agingThreshold);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AgingSchemeId = new SelectList(db.AgingSchemes, "AgingSchemeId", "SchemeName", agingThreshold.AgingSchemeId);
            return View(agingThreshold);
        }

        // GET: AgingThresholds/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgingThreshold agingThreshold = db.AgingThresholds.Find(id);
            if (agingThreshold == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgingSchemeId = new SelectList(db.AgingSchemes, "AgingSchemeId", "SchemeName", agingThreshold.AgingSchemeId);
            return View(agingThreshold);
        }

        // POST: AgingThresholds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AgingThresholdId,AgingSchemeId,ThresholdName,DaysPrior,IncrementRate,LastUpdated")] AgingThreshold agingThreshold)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agingThreshold).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgingSchemeId = new SelectList(db.AgingSchemes, "AgingSchemeId", "SchemeName", agingThreshold.AgingSchemeId);
            return View(agingThreshold);
        }

        // GET: AgingThresholds/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgingThreshold agingThreshold = db.AgingThresholds.Find(id);
            if (agingThreshold == null)
            {
                return HttpNotFound();
            }
            return View(agingThreshold);
        }

        // POST: AgingThresholds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AgingThreshold agingThreshold = db.AgingThresholds.Find(id);
            db.AgingThresholds.Remove(agingThreshold);
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
