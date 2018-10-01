using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace InventoryManager.Controllers
{
    public class StoresController : BaseController
    {
        private InventoryManagerEntities db = new InventoryManagerEntities();

        // GET: Stores
        public ActionResult Index()
        {
            return View(db.tbl_Stores.ToList());
        }

        // GET: Stores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Guid,Name,Description,Address,City,State,Zipcode,PhoneNumber,EmailAddress,Logo,LogoName,DateCreated,Active")] tbl_Stores tbl_Stores)
        {
            if (ModelState.IsValid)
            {
                tbl_Stores.Guid = Guid.NewGuid();
                tbl_Stores.DateCreated = DateTime.Now;
                tbl_Stores.Active = true;
                db.tbl_Stores.Add(tbl_Stores);
                db.SaveChanges();
                return RedirectToAction("Index", "Items", new { guid = tbl_Stores.Guid });
            }

            return View(tbl_Stores);
        }

        // GET: Stores/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Stores tbl_Stores = db.tbl_Stores.Find(id);
            if (tbl_Stores == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Stores);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Guid,Name,Description,Address,City,State,Zipcode,PhoneNumber,EmailAddress,Logo,LogoName,DateCreated,Active")] tbl_Stores tbl_Stores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Stores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Items", new { guid = tbl_Stores.Guid});
            }
            return View(tbl_Stores);
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
