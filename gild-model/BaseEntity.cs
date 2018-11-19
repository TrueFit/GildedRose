using Newtonsoft.Json;
using System;

namespace gild_model
{
    public abstract class BaseEntity : IBaseEntity
    {
        [JsonProperty("contractName")]
        public abstract string ContractName { get; }

        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("createdDateUtc")]
        public DateTime CreatedDateUtc { get; set; } = DateTime.UtcNow;
    }
}
