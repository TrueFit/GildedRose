using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GlidedRose.Controllers
{
  [ApiController]
  [Route("inventory")]
  public class InventoryController : ControllerBase
  {
    private readonly GlidedRoseApp _app;

    private readonly ILogger<InventoryController> _logger;

    public InventoryController(GlidedRoseApp app, ILogger<InventoryController> logger)
    {
      _logger = logger;
      _app = app;
    }

    [HttpGet]
    [Route("goods")]
    public IEnumerable<Item> GetAll()
    {
      return _app.GetList();
    }

    [HttpGet]
    [Route("goods/{name}")]
    public Item Get(string name)
    {
      return _app.GetItem(name);
    }

    [HttpPost]
    [Route("progress-day")]
    public IActionResult ProgressDay()
    {
      _app.ProgressDay();
      return Ok();
    }

    [HttpGet]
    [Route("trashed")]
    public IEnumerable<Item> GetTrashed()
    {
      return _app.GetTrashedItems();
    }
  }
}
