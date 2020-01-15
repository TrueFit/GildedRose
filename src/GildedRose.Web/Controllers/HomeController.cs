using Microsoft.AspNetCore.Mvc;

namespace GildedRose.Web.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}