using GildedRose.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GildedRose.Controllers
{
    public class ProductAgingController : Controller
    {
        public ActionResult Index()
        {
            var db = new GildedRoseContext();
            var retList = db.ProductAgingList(DateTime.Now, DateTime.Now, null).ToList();

            return View(retList);
        }
    }
}