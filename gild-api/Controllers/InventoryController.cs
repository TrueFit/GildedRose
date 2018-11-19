using gild_repo;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace gild_api.Controllers
{
    public class InventoryController : ApiController
    {
        private readonly FileSystemConnectionContext _connectionContext;
        private readonly IInventoryRepo _inventoryRepo;

        public InventoryController(IInventoryRepo inventoryRepo)
        {
            _connectionContext = new FileSystemConnectionContext(HttpContext.Current.Server.MapPath("~/App_Data/"));
            _inventoryRepo = inventoryRepo;
        }

        [HttpGet]
        [HttpPost]
        [Route("api/get-inventory")]
        public HttpResponseMessage GetInventory()
        {
            try
            {
                /// TODO: Use a view-model.
                return Request.CreateResponse(_inventoryRepo.Get(_connectionContext));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to load inventory.");
            }
        }

        public class GetInventoryItemServiceModel
        {
            public string ItemName { get; set; }
        }

        [HttpGet]
        [HttpPost]
        [Route("api/get-inventory-item")]
        public HttpResponseMessage GetInventoryItem()
        {
            try
            {
                // TODO: Use a view-model.
                return Request.CreateResponse(_inventoryRepo.Get(_connectionContext).First());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to load inventory.");
            }
        }

        [HttpGet]
        [HttpPost]
        [Route("api/get-trash")]
        public HttpResponseMessage GetTrash()
        {
            try
            {
                return Request.CreateResponse(_inventoryRepo.GetTrash(_connectionContext));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to load inventory.");
            }
        }
    }
}
