using GildedRose.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GildedRose.Controllers
{
    public class HomeController : Controller
    {
        private static GildedRoseContext _db = new GildedRoseContext();
        public ActionResult Index()
        {
            var viewModel = new ProductAgingViewModel() { ProductAges = new List<ProductAgingList>(), AsOfDate = DateTime.Now, ZeroOnly = false, Products = new List<ProductList>() };

            viewModel.ProductAges = _db.ProductAgingList(DateTime.Now, DateTime.Now, null).ToList();

             var lookups = from t in viewModel.ProductAges
                           orderby t.ProductName ascending
                             select new ProductList
                             {
                                 ProductId = t.ProductId,
                                 ProductName = t.ProductName.ToString()
                             };

            viewModel.Products = lookups.ToList();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(ProductAgingViewModel model)
        {
            var viewModel = new ProductAgingViewModel() { ProductAges = new List<ProductAgingList>(), Products = new List<ProductList>(), ZeroOnly=false };

            var ageList = _db.ProductAgingList(DateTime.Now, model.AsOfDate, null).ToList();
        
            var lookups = from t in ageList
                             orderby t.ProductName ascending
                             select new ProductList
                             {
                                 ProductId = t.ProductId,
                                 ProductName = t.ProductName.ToString()
                             };

            viewModel.Products = lookups.ToList();

            if (model.ProductId.HasValue)
                ageList = ageList.Where(x => x.ProductId == model.ProductId).ToList();
            else
            {
                if (model.ZeroOnly)
                {
                    ageList = ageList.Where(x => x.RemainingDays < 1).OrderBy(y => y.RemainingDays).ToList();
                }
            }

            viewModel.ProductAges = ageList.ToList();

            return View(viewModel);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Charles Burris";

            return View();
        }
    }
}