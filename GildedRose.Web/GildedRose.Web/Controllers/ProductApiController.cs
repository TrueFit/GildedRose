
using GildedRose.BusinessObjects;
using GildedRose.ClientHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GildedRose.Web.Controllers
{
    [RoutePrefix("api/productApi")]
    public class ProductApiController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            IHttpActionResult ret;

            var helper = new InventoryClientHelpers();
            var task = helper.GetInventoryItems();
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

        [Route("Search/{name}")]
        [HttpGet()]
        public IHttpActionResult Search(string name)
        {
            IHttpActionResult ret;

            var helper = new InventoryClientHelpers();
            var task = helper.GetItemByName(name);
            var item = helper.InventoryItem;
            var items = new List<InventoryItem>();
            items.Add(item);

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

        [Route("Zero")]
        [HttpGet()]
        public IHttpActionResult GetZeroItems()
        {
            IHttpActionResult ret;

            var helper = new InventoryClientHelpers();
            var task = helper.GetZeroQualityInventoryItems();
            var items = helper.InventoryItems;

            // we can expect to get some empty results and we don't want to blow up the return
            if (task.Exception != null)
            {
                ret = BadRequest(task.Exception.Message);
            }
            else
            {
                ret = Ok(items);
            }

            return ret;
        }

        [Route("Reset")]
        [HttpGet()]
        public IHttpActionResult ResetAllItems()
        {
            IHttpActionResult ret;

            var helper = new InventoryClientHelpers();
            var task = helper.ResetToInitialState();
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