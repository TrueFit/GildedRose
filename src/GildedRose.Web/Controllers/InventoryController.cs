#region Using Directives

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GildedRose.Domain.Inventory;
using GildedRose.Web.Models;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace GildedRose.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        /// <summary>
        /// Get a list of all  <see cref="Inventory" />
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var inventory = await _inventoryRepository.GetList();
            return new JsonResult(inventory);
        }

        /// <summary>
        /// Get a list of  <see cref="Inventory" /> filtered by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{name}/filter")]
        public async Task<IActionResult> GetByName(string name)
        {
            var inventory = await _inventoryRepository.GetByName(name);
            return new JsonResult(inventory);
        }

        /// <summary>
        /// Get a list of  <see cref="Inventory" /> having Quality = 0
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("trash")]
        public async Task<IActionResult> GetTrash()
        {
            var inventory = await _inventoryRepository.GetTrash();
            return new JsonResult(inventory);
        }

        /// <summary>
        /// Get a list of all  <see cref="InventoryHistory" />
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("history")]
        public async Task<IActionResult> GetHistory()
        {
            var history = await _inventoryRepository.GetHistory();
            return new JsonResult(history);
        }

        /// <summary>
        /// Run the inventory aging process and advance aging by one day
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("aging")]
        public async Task<IActionResult> PerformAging()
        {
            var responseModel = new ResponseModel();
            try
            {
                var result = await _inventoryRepository.PerformAging();
                responseModel.Status = ResponseStatus.Success;
                responseModel.Message = "Inventory aging was successful";
            }
            catch (Exception ex)
            {
                responseModel.Status = ResponseStatus.Failure;
                responseModel.Message = "An error occurred during the aging process";
                Debug.WriteLine($"aging exception: {ex.Message}::{ex.StackTrace}");
            }

            return new JsonResult(responseModel);
        }
    }
}
