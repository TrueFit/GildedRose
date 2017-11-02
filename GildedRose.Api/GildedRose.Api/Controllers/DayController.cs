using System.Collections.Generic;
using System.Web.Http;
using GildedRose.Entities;
using GildedRose.Services;
using GildedRose.BusinessObjects;

namespace GildedRose.Api.Controllers
{
    [RoutePrefix("api/Day")]
    public class DayController : ApiController
    {
        private readonly InventoryDayServices _service;

        public DayController(GildedRoseEntities context)
        {
            _service = new InventoryDayServices(context);
        }

        public DayController()
        {
            var context = new GildedRoseEntities();
            _service = new InventoryDayServices(context);
        }

        // GET api/Day
        [HttpGet]
        [Route("{numDays}")]
        public IEnumerable<InventoryItem> EndDay(int numDays) => 
            _service.IncrementDay(numDays);

    }
}
