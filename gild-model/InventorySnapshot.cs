using Newtonsoft.Json;
using System.Collections.Generic;

namespace gild_model
{
    public class InventorySnapshot : Snapshot
    {
        [JsonProperty("inventoryItems")]
        public List<InventoryItem> InventoryItems { get; set; }

        [JsonProperty("hasProcessedInitialInventory")]
        public bool HasProcessedInitialInventory { get; set; }

        public override string ContractName => "inventory-snapshot";
    }
}
