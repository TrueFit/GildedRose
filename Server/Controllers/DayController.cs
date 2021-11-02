using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GildedRose_Blazor.Server.Helpers;
using System;
using GildedRose_Blazor.Shared;

namespace GildedRose_Blazor.Server.Controllers
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

        //Increment The day(s)
        [HttpGet("AdvanceDays")]
        public async Task<ActionResult> AdvanceDays(int days)
        {
            //make sure at least 1 day is incrmeented
            if(days <= 0) days = 1;
            try
            {
                //try to increment the day. Method resolves true or false
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
