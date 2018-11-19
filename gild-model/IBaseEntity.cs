using Newtonsoft.Json;
using System;

namespace gild_model
{
    public interface IBaseEntity
    {
        [JsonProperty("contractName")]
        string ContractName { get; }

        [JsonProperty("id")]
        Guid Id { get; set; }

        [JsonProperty("createdDateUtc")]
        DateTime CreatedDateUtc { get; set; }
    }
}
