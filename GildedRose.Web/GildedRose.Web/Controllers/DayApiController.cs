using GildedRose.BusinessObjects;
using GildedRose.ClientHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GuildedRose.Web.Controllers
{
    [RoutePrefix("api/dayApi")]
    public class DayApiController : ApiController
    {
        [Route("EndDay")]
        [HttpGet()]
        public IHttpActionResult EndDay()
        {
            IHttpActionResult ret;

            var helper = new DayClientHelpers();
            var task = helper.EndDay();
            var items = helper.InventoryItems;

            if (items != null && items.Count() > 0)
            {
                ret = Ok(items);
            }
            else if (task.Exception != null)
            {
                ret = BadRequest(task.Exception.Message);
            }
            else
            {
                ret = NotFound();
            }

            return ret;
        }
    }
}
