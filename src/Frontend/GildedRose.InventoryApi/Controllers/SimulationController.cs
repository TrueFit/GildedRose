using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GildedRose.InventoryApi
{
    public class SimulationController : Controller
    {
        #region Construct and Dependencies
        public SimulationController(GR.Managers.SimulationInventoryManager simulationManager)
        {
            SimulationManager = simulationManager
                ?? throw new ArgumentNullException(nameof(simulationManager));
        }

        private GR.Managers.SimulationInventoryManager SimulationManager { get; set; }
        #endregion

        [HttpGet]
        public DateTime GetDate()
            => SimulationManager.Now;

        [HttpPost]
        public Task<GR.Models.ProcessResults.NightlyQualityUpdateResult> SetDate(DateTime date)
            => SimulationManager.SetDate(date);

        [HttpPost]
        public Task<GR.Models.ProcessResults.NightlyQualityUpdateResult> AdvanceDateByOneDay()
            => SimulationManager.AdvanceDateByOneDay();
    }
}
