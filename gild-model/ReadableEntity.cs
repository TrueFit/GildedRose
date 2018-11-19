using System;

namespace gild_model
{
    public class ReadableEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public string ContractName { get; set; }
    }
}
