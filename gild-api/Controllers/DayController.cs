using gild_repo;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace gild_api.Controllers
{
    public class DayController : ApiController
    {
        private readonly FileSystemConnectionContext _connectionContext;
        private readonly IDayRepo _dayRepo;        

        public DayController(IDayRepo dayRepo)
        {
            _connectionContext = new FileSystemConnectionContext(HttpContext.Current.Server.MapPath("~/App_Data/"));
            _dayRepo = dayRepo;
        }

        [HttpGet]
        [HttpPost]
        [Route("api/advance-day")]
        public HttpResponseMessage AdvanceDay()
        {
            try
            {
                _dayRepo.AdvanceDay(_connectionContext);
                return Request.CreateResponse(HttpStatusCode.OK, _dayRepo.GetCurrentDay(_connectionContext) + 1);
            }
            catch (Exception exception)
            {
                /// TODO: Log this.
                Console.WriteLine(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to advance day");
            }
        }

        [HttpGet]
        [HttpPost]
        [Route("api/get-day")]
        public HttpResponseMessage GetDay()
        {
            var currentDay = _dayRepo.GetCurrentDay(_connectionContext);
            return Request.CreateResponse(HttpStatusCode.OK, currentDay + 1);
        }
    }
}
