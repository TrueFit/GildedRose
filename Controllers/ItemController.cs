using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using GildedRoseAPI.Services;
using GildedRoseAPI.Requests;
using GildedRoseAPI.Responses;

namespace GuildedRoseWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class ItemController : ControllerBase
    {
        private readonly ItemService _itemService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ItemController> _logger;

        public ItemController(ItemService itemService, IConfiguration configuration, ILogger<ItemController> logger)
        {
            _itemService = itemService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<GenericResponse> ResetItems()
        {
            return _itemService.ResetItems();
        }

        [HttpGet]
        public ActionResult<ItemListResponse> GetItems()
        {
            return _itemService.GetItems();
        }

        [HttpGet]
        public ActionResult<ItemListResponse> GetItemsToTrash()
        {
            return _itemService.GetItemsToTrash();
        }

        [HttpPost]
        public ActionResult<ItemResponse> GetItem(ItemRequest request)
        {
            return _itemService.GetItem(request);
        }

        [HttpPost]
        public ActionResult<GenericResponse> TrashAll()
        {
            return _itemService.TrashAll();
        }

        /*
        [HttpPost]
        public ActionResult<GenericResponse> TrashItem(ItemRequest request)
        {
            return _itemService.TrashItem(request);
        }
        */

        [HttpPost]
        public ActionResult<GenericResponse> ProgressToNextDay()
        {
            return _itemService.ProgressToNextDay();
        }
    }
}
