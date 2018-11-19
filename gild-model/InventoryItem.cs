using Newtonsoft.Json;
using System;

namespace gild_model
{
    public class InventoryItem
    {
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("sellIn")]
        public decimal SellIn { get; set; }

        [JsonProperty("quality")]
        public decimal Quality { get; set; }        
    }
}
