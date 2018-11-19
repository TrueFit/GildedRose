using gild_model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gild_repo
{
    public interface IInventorySnapshotProcessor : ISnapshotProcessor<InventorySnapshot>
    {        
    }

    public class InventorySnapshotProcessor : IInventorySnapshotProcessor
    {
        public void ProcessEvent(InventorySnapshot snapshot, string eventContents, DateTime snapshotFileCreationTime)
        {
            var readableEntity = JsonConvert.DeserializeObject<ReadableEntity>(eventContents);

            if (string.Equals(readableEntity.ContractName, new SetInitialInventoryDataEvent().ContractName, StringComparison.InvariantCultureIgnoreCase))
            {
                ProcessSetInitialInventory(snapshot, JsonConvert.DeserializeObject<SetInitialInventoryDataEvent>(eventContents));
            }
            else if (string.Equals(readableEntity.ContractName, new AdvanceDayDataEvent().ContractName, StringComparison.InvariantCultureIgnoreCase))
            {
                ProcessAdvanceDay(snapshot, JsonConvert.DeserializeObject<AdvanceDayDataEvent>(eventContents));
            }

            // Sulfuras' quality should always be 80.
            if (snapshot?.InventoryItems != null)
            {
                snapshot.InventoryItems.Where(item => string.Equals(item.Category, "Sulfuras", StringComparison.InvariantCultureIgnoreCase))
                    .ToList()
                    .ForEach(item => item.Quality = 80);
            }                

            snapshot.LastFileCreationTimeUtc = snapshotFileCreationTime;
        }       

        private void ProcessSetInitialInventory(InventorySnapshot snapshot, SetInitialInventoryDataEvent dataEvent)
        {
            if (!snapshot.HasProcessedInitialInventory)
            {
                snapshot.InventoryItems = dataEvent.InventoryItems;
                snapshot.HasProcessedInitialInventory = true;
            }
        }

        private void ProcessAdvanceDay(InventorySnapshot snapshot, AdvanceDayDataEvent dataEvent)
        {
            // Don't advance the day if the initial inventory hasn't been set.
            if (!snapshot.HasProcessedInitialInventory) { return; }

            foreach (var item in snapshot.InventoryItems ?? new List<InventoryItem>())
            {
                // TODO: This needs to move to a ruleset.
                int qualityChange = 0;

                if (string.Equals(item.Name, "Aged Brie", StringComparison.InvariantCultureIgnoreCase))
                {
                    qualityChange = item.SellIn > 0 ? 1 : 2;
                }
                else if (string.Equals(item.Category, "Backstage Passes", StringComparison.InvariantCultureIgnoreCase))
                {
                    // "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches;
                    // Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or less but
                    // Quality drops to 0 after the concert
                    if (item.SellIn <= 0)
                    {
                        item.Quality = 0;
                    }
                    else if (item.SellIn <= 5)
                    {
                        qualityChange = 3;
                    }
                    else if (item.SellIn <= 10)
                    {
                        qualityChange = 2;
                    }
                    else
                    {
                        qualityChange = 1;
                    }
                }
                else if (string.Equals(item.Category, "Conjured", StringComparison.InvariantCultureIgnoreCase))
                {
                    qualityChange = item.SellIn > 0 ? -2 : -4;
                }
                else if (string.Equals(item.Category, "Sulfuras", StringComparison.InvariantCultureIgnoreCase))
                {
                }
                else
                {
                    qualityChange = item.SellIn > 0 ? -1 : -2;
                }

                var updatedQuality = item.Quality + qualityChange;

                if (updatedQuality < 0) { updatedQuality = 0; }
                if (updatedQuality > 50) { updatedQuality = 50; }

                item.Quality = updatedQuality;

                if (item.SellIn > 0)
                {
                    item.SellIn--;
                }
            }
        }
    }
}
