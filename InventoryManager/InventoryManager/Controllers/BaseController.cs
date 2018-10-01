using System.Web.Mvc;

namespace InventoryManager.Controllers
{
    public abstract partial class BaseController : Controller
    {
        public InventoryManagerEntities db = new InventoryManagerEntities();
        public tbl_Stores currentStore = System.Web.HttpContext.Current.Session["currentStore"] != null ? (tbl_Stores)System.Web.HttpContext.Current.Session["currentStore"] : null;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Store = currentStore;
            base.OnActionExecuting(filterContext);
        }
    }
}