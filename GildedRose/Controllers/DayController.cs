using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccessLibrary;
using Microsoft.AspNetCore.Mvc;
using GildedRose.Helpers;
using System;

namespace GildedRose.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DayController : ControllerBase
    {
        private readonly InventoryContext _context;

        public DayController(InventoryContext context)
        {
            _context = context;
        }

        //Add a single item
        [HttpPut("AdvanceDays")]
        public async Task<ActionResult> AdvanceDays(int days)
        {
            try
            {
                //try to increment one day, method resolves true or false
                if (await new IncrementDayHandler(_context).IncrementDays(days))
                    return Ok();
                else
                    return BadRequest("error: The day(s) did not advance");
            }
            catch (Exception e)
            {
                return BadRequest("error: " + e.Message);
            }
        }

    }
}
