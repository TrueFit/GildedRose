using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace GildedRose.Controllers
{
    public class HomeController : Controller
    {
        public MongoContext GildedRoseContext = new MongoContext();
        
        public HomeController()
        {
          
        }

        public IActionResult Index()
        {
            var junk = GildedRoseContext.Database.Settings.ToString();
            return Json(junk);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
