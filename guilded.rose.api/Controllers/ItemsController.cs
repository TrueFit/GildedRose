using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using guilded.rose.api.Domain;
using guilded.rose.api.Domain.Business.Interfaces;
using guilded.rose.api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace guilded.rose.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemBL _itemBL;

        public ItemsController(IItemBL itemBL)
        {
            _itemBL = itemBL;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> Get()
        {
            // TODO: Layout Angular

            var items = await _itemBL.GetItems();
            return Ok(items);
        }

        [HttpGet]
        [Route("daily")]
        public async Task<ActionResult<IEnumerable<Item>>> GetDailyItems(DateTime date)
        {
            var items = await _itemBL.GetDailyItems(date);
            return Ok(items);
        }

        [HttpGet]
        [Route("trash/{date}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetDailyTrash(DateTime date)
        {
            var items = await _itemBL.GetDailyTrash(date);
            return Ok(items);
        }
        [HttpGet]
        [Route("{name}")]
        public async Task<ActionResult<Item>> GetByName(string name)
        {
            var item = await _itemBL.GetItemByName(name);
            
            return Ok(item);
        }
    }
}