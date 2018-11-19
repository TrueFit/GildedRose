using gild_model;
using System;

namespace gild_repo
{
    public interface ISnapshotProcessor<TSnapshot>
        where TSnapshot : Snapshot
    {
        void ProcessEvent(TSnapshot snapshot, string eventContents, DateTime snapshotFileCreationTime);        
    }
}
