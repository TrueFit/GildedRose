using System.Linq;
using System.Web.Mvc;

namespace InventoryManager.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            // clear the store that is set in the Base Controller
            System.Web.HttpContext.Current.Session["currentStore"] = null;
            ViewBag.Store = null;

            // start Quartz Scheduler Job
            JobSchedulerController JS = new JobSchedulerController();
            JS.Start();

            return View(db.tbl_Stores.Where(x => x.Active == true).ToList());
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Admin(string message)
        {
            ViewBag.Message = message;
            ViewBag.Log = db.tbl_DailyUpdateLog.OrderByDescending(x => x.DateTime).FirstOrDefault();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Designed and developed by...";

            return View();
        }
    }
}