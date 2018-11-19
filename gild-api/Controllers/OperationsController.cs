using gild_repo;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace gild_api.Controllers
{
    public class OperationsController : ApiController
    {
        private readonly FileSystemConnectionContext _connectionContext;
        private readonly string _appDataFolder;
        private readonly IInventoryRepo _inventoryRepo;

        public OperationsController(IInventoryRepo inventoryRepo)
        {
            _appDataFolder = HttpContext.Current.Server.MapPath("~/App_Data/");
            _connectionContext = new FileSystemConnectionContext(_appDataFolder);
            _inventoryRepo = inventoryRepo;
        }

        [HttpGet]
        [HttpPost]
        [Route("api/reset-the-universe")]
        public HttpResponseMessage ResetTheUniverse()
        {
            try
            {
                var directoryInfo = new DirectoryInfo(_appDataFolder);
                directoryInfo.GetFiles().ToList().ForEach(queryFile => queryFile.Delete());
                _inventoryRepo.InitializeData(_connectionContext);
                
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to reset the universe");
            }
            
        }        
    }
}
