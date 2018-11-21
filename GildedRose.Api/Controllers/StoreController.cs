using System.Collections.Generic;
using System.Linq;
using GildedRose.Model;
using GildedRose.Services;
using GildedRose.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GildedRose.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _service;

        public StoreController(IStoreService service)
        {
            _service = service;
        }

        // GET api/store
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<List<StoreItemDto>> Get()
        {
            List<StoreItemDto> items = _service.GetInventory().ToList();

            if (items.Any())
                return Ok(items);
            else
            {
                // ...otherwise, return a 404 status code 
                // ControllerBase.NotFound Method does a return new NotFoundResult();
                return NotFound();
            }
        }

        // GET api/store/name
        [HttpGet("{name}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<List<StoreItemDto>> Get(string name)
        {
            var item = _service.GetInventoryItem(name);
            if (item == null)
            {
                // ControllerBase.NotFound Method
                // Returns a 404 status code as if you did the following:
                // return new NotFoundResult();
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/store
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Create(StoreItemDto item)
        {
            if (!ItemExists(item.Name))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _service.AddItem(item);

                try
                {
                    _service.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    // Returns a 409 status - The request could not be completed due to a conflict with the current state of the resource.
                    return Conflict(e.InnerException.ToString());
                }
            }
            return Ok();
        }

        // PUT api/store/
        [HttpPut("{name}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public IActionResult Put(string name, [FromBody] StoreItemDto item)
        {
            if (ItemExists(item.Name))
            {
                if (!ModelState.IsValid)
                {
                    // 409
                    return Conflict(ModelState);
                }

                _service.UpdateItem(item);

                try
                {
                    _service.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(name))
                    {
                        // 404
                        return NotFound();
                    }
                    else
                    {
                        // Returns a 409 status - The request could not be completed due to a conflict with the current state of the resource. .
                        return Conflict();
                    }
                }
                // 200
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE api/store/
        [HttpDelete("{name}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(string name)
        {
            var item = _service.GetInventoryItem(name);
            if (item == null)
            {
                // ControllerBase.NoContent Method
                // Returns a 404 status code as if you did the following:
                // return new NotFoundResult();
                return NotFound();
            }

            _service.DeleteItem(item);
            _service.SaveChanges();

            // From the ControllerBase.NoContent Method
            // Returns a 204 status - i.e. the server has fulfilled 
            // the request but does not need to return an entity-body
            return NoContent();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Queries if a given item exists. </summary>
        ///
        /// <param name="name"> The item record name. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        private bool ItemExists(string name)
        {
            return _service.GetInventoryItem(name) != null;
        }
    }
}
