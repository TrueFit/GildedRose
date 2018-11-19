using System;

namespace gild_model
{
    public abstract class Snapshot : DataEvent
    {
        public DateTime? LastFileCreationTimeUtc { get; set; }
    }
}
