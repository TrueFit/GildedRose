using System.Collections.Generic;

namespace gild_model
{
    // This event is only processed up to one time.
    // Addtional events have no effect on the snapshot.
    public class SetInitialInventoryDataEvent : DataEvent
    {
        public List<InventoryItem> InventoryItems { get; set; }

        public override string ContractName => "set-initial-inventory";
    }
}
