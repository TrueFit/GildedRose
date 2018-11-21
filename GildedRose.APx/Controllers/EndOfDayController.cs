using System.Collections.Generic;
using System.Linq;
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

        // POST api/store
        [HttpPost]
        public void Post(string value)
        {
            if (value == "yes")
                _service.ProcessEndOfDay();
        }
    }
   }