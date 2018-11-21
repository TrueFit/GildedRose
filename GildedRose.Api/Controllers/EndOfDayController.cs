using System.Collections.Generic;
using System.Linq;
using GildedRose.Model;
using GildedRose.Services;
using GildedRose.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GildedRose.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndOfDayController : ControllerBase
    {
        private readonly IEndOfDayService _service;

        public EndOfDayController(IEndOfDayService service)
        {
            _service = service;
        }

        [HttpGet("GetListofTrash")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<List<StoreItemDto>> GetListofTrash()
        {
            List<StoreItemDto> items = _service.GetListofTrash().ToList();

            if (items.Any())
                return Ok(items);
            else
            {
                // ...otherwise, return a 404 status code 
                // ControllerBase.NotFound Method does a return new NotFoundResult();
                return NotFound();
            }
        }


        // POST api/store
        [HttpPost]
        public void Post(string value)
        {
            if (value == "yes")
            {
                _service.ProcessEndOfDay();
                _service.SaveChanges();
            }

        }
    }
}