using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;

namespace InventoryManager.Controllers
{
    public class ItemsController : BaseController
    {
        // GET: Items
        public ActionResult Index(Guid guid, string sortOrder, string currentFilter, string searchString, int? page)
        {
            // get active items for this store only
            var tbl_Items = db.tbl_Items.Where(x => x.StoreGuid == guid && x.Active == true && x.Quality > 0 && (x.DateTrashed >= DateTime.Now || x.DateTrashed == null)).Include(t => t.tbl_Stores);

            // set store in session
            currentStore = db.tbl_Stores.Where(x => x.Guid == guid).FirstOrDefault();
            System.Web.HttpContext.Current.Session["currentStore"] = currentStore;
            ViewBag.Store = currentStore;

            // Search Name
            if (!String.IsNullOrEmpty(searchString))
            {
                tbl_Items = tbl_Items.Where(s => s.Name.Contains(searchString) || s.Category.Contains(searchString));
            }
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            // sorting
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.SellInSortParm = sortOrder == "SellIn" ? "SellIn_desc" : "SellIn";
            ViewBag.QualitySortParm = sortOrder == "Quality" ? "Quality_desc" : "Quality";
            ViewBag.LegendarySortParm = sortOrder == "Legendary" ? "SellIn_desc" : "Legendary";
            ViewBag.BetterWithAgeSortParm = sortOrder == "BetterWithAge" ? "BetterWithAge_desc" : "BetterWithAge";
            switch (sortOrder)
            {
                case "Name":
                    tbl_Items = tbl_Items.OrderBy(s => s.Name);
                    break;
                case "Name_desc":
                    tbl_Items = tbl_Items.OrderByDescending(s => s.Name);
                    break;
                case "Category":
                    tbl_Items = tbl_Items.OrderBy(s => s.Category);
                    break;
                case "Category_desc":
                    tbl_Items = tbl_Items.OrderByDescending(s => s.Category);
                    break;
                case "SellIn":
                    tbl_Items = tbl_Items.OrderBy(s => s.SellIn);
                    break;
                case "SellIn_desc":
                    tbl_Items = tbl_Items.OrderByDescending(s => s.SellIn);
                    break;
                case "Quality":
                    tbl_Items = tbl_Items.OrderBy(s => s.Quality);
                    break;
                case "Quality_desc":
                    tbl_Items = tbl_Items.OrderByDescending(s => s.Quality);
                    break;
                case "Legendary":
                    tbl_Items = tbl_Items.OrderBy(s => s.Legendary);
                    break;
                case "Legendary_desc":
                    tbl_Items = tbl_Items.OrderByDescending(s => s.Legendary);
                    break;
                case "BetterWithAge":
                    tbl_Items = tbl_Items.OrderBy(s => s.BetterWithAge);
                    break;
                case "BetterWithAge_desc":
                    tbl_Items = tbl_Items.OrderByDescending(s => s.BetterWithAge);
                    break;
                default:
                    tbl_Items = tbl_Items.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(tbl_Items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Trash Items
        public ActionResult Trash(Guid guid, string sortOrder, string currentFilter, string searchString, int? page)
        {
            // get quality = 0 items for this store only
            var tbl_Items = db.tbl_Items.Where(x => x.StoreGuid == guid && x.Active == true && x.Quality == 0).Include(t => t.tbl_Stores);

            // set store in session
            currentStore = db.tbl_Stores.Where(x => x.Guid == guid).FirstOrDefault();
            System.Web.HttpContext.Current.Session["currentStore"] = currentStore;
            ViewBag.Store = currentStore;

            // Search Name
            if (!String.IsNullOrEmpty(searchString))
            {
                tbl_Items = tbl_Items.Where(s => s.Name.Contains(searchString) || s.Category.Contains(searchString));
            }
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            // sorting
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.CategorySortParm = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewBag.SellInSortParm = sortOrder == "SellIn" ? "SellIn_desc" : "SellIn";
            ViewBag.QualitySortParm = sortOrder == "Quality" ? "Quality_desc" : "Quality";
            ViewBag.LegendarySortParm = sortOrder == "Legendary" ? "SellIn_desc" : "Legendary";
            ViewBag.BetterWithAgeSortParm = sortOrder == "BetterWithAge" ? "BetterWithAge_desc" : "BetterWithAge";
            switch (sortOrder)
            {
                case "Name":
                    tbl_Items = tbl_Items.OrderBy(s => s.Name);
                    break;
                case "Name_desc":
                    tbl_Items = tbl_Items.OrderByDescending(s => s.Name);
                    break;
                case "Category":
                    tbl_Items = tbl_Items.OrderBy(s => s.Category);
                    break;
                case "Category_desc":
                    tbl_Items = tbl_Items.OrderByDescending(s => s.Category);
                    break;
                case "SellIn":
                    tbl_Items = tbl_Items.OrderBy(s => s.SellIn);
                    break;
                case "SellIn_desc":
                    tbl_Items = tbl_Items.OrderByDescending(s => s.SellIn);
                    break;
                case "Quality":
                    tbl_Items = tbl_Items.OrderBy(s => s.Quality);
                    break;
                case "Quality_desc":
                    tbl_Items = tbl_Items.OrderByDescending(s => s.Quality);
                    break;
                case "Legendary":
                    tbl_Items = tbl_Items.OrderBy(s => s.Legendary);
                    break;
                case "Legendary_desc":
                    tbl_Items = tbl_Items.OrderByDescending(s => s.Legendary);
                    break;
                case "BetterWithAge":
                    tbl_Items = tbl_Items.OrderBy(s => s.BetterWithAge);
                    break;
                case "BetterWithAge_desc":
                    tbl_Items = tbl_Items.OrderByDescending(s => s.BetterWithAge);
                    break;
                default:
                    tbl_Items = tbl_Items.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(tbl_Items.ToPagedList(pageNumber, pageSize));
        }

        // clear all trash
        public ActionResult TrashAll(Guid guid)
        {
            // get trash items for this store only
            var tbl_Items = db.tbl_Items.Where(x => x.StoreGuid == guid && x.Active == true && x.Quality == 0).Include(t => t.tbl_Stores);
            
            foreach (tbl_Items item in tbl_Items.ToList())
            {
                // deactive trash items
                item.Active = false;
                item.DateTrashed = DateTime.Now;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }

            tbl_Items = db.tbl_Items.Where(x => x.StoreGuid == guid && x.Active == true && x.Quality == 0).Include(t => t.tbl_Stores);
            return RedirectToAction("Trash", new { guid = currentStore.Guid });
        }

        // clear all trash
        public ActionResult Sell(Guid guid)
        {
            // get trash items for this store only
            var tbl_Items = db.tbl_Items.Where(x => x.Guid == guid);

            foreach (tbl_Items item in tbl_Items.ToList())
            {
                // deactive sold items
                item.Active = false;
                item.DateSold = DateTime.Now;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index", new { guid = currentStore.Guid });
        }

        // to run the Daily Update Job manually
        public ActionResult DailyUpdate()
        {
            JobSchedulerController JS = new JobSchedulerController();
            if (JS.DailyUpdateAlgorithm(true))
            {
                // ran successfully
                return RedirectToAction("Admin", "Home", new { message = "Daily Update Successfully Run!" });
            }
            else
            {
                // failed to complete
                tbl_DailyUpdateLog log = db.tbl_DailyUpdateLog.OrderByDescending(x => x.DateTime).FirstOrDefault();
                string msg = "Daily Update Failed! Only runs once per day! Last Run: " + log.DateTime;
                return RedirectToAction("Admin", "Home", new { message = msg });
            } 
        }


        // GET: Items/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Guid,StoreGuid,Name,Category,SellIn,Quality,Legendary,BetterWithAge,Image,ImageName,DateCreated,DateTrashed,Active")] tbl_Items tbl_Items)
        {
            if (ModelState.IsValid)
            {
                tbl_Items.Guid = Guid.NewGuid();
                tbl_Items.StoreGuid = currentStore.Guid;
                tbl_Items.DateCreated = DateTime.Now;
                if (tbl_Items.Name == "Aged Brie")
                    { tbl_Items.BetterWithAge = true; }
                if (tbl_Items.Category == "Sulfuras")
                    { tbl_Items.Legendary = true; }
                tbl_Items.Active = true;
                db.tbl_Items.Add(tbl_Items);
                db.SaveChanges();
                return RedirectToAction("Index", new { guid = currentStore.Guid });
            }

            return View(tbl_Items);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Items tbl_Items = db.tbl_Items.Find(id);
            if (tbl_Items == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Items);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Guid,StoreGuid,Name,Category,SellIn,Quality,PurchasePrice,SellPrice,Legendary,BetterWithAge,Image,ImageName,DateCreated,DateTrashed,Active")] tbl_Items tbl_Items)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Items).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { guid = currentStore.Guid });
            }
            return View(tbl_Items);
        }

        // GET: Upload
        public ActionResult Upload()
        {
            return View();
        }

        // POST: Upload
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(string file)
        {
            try
            {
                using (StringReader reader = new StringReader(file))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] itemimport = line.Split(',');

                        tbl_Items item = new tbl_Items();
                        item.Guid = Guid.NewGuid();
                        item.Active = true;
                        item.Category = itemimport[1];
                        item.DateCreated = DateTime.Now;
                        item.Name = itemimport[0];
                        if(item.Name == "Aged Brie")
                            { item.BetterWithAge = true; }
                        if (item.Category == "Sulfuras")
                            { item.Legendary = true; }
                        item.Quality = Int32.Parse(itemimport[2]);
                        item.SellIn = Int32.Parse(itemimport[3]);
                        item.StoreGuid = currentStore.Guid;
                        db.tbl_Items.Add(item);
                        db.SaveChanges();
                    }
                }
           
                ViewBag.Message = "Inventory Uploaded Successfully!";
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Message = "Inventory upload failed! " + e.Message;
                return View();
            }
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
