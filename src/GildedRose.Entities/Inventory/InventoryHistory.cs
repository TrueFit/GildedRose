using System;
using System.ComponentModel.DataAnnotations;

namespace GildedRose.Entities.Inventory
{
    /// <summary>
    /// Action driving the update of an inventory item in the system.
    /// </summary>
    public enum InventoryHistoryAction
    {
        Created = 1,
        Modified = 2,
        Aged = 3,
        Sold = 4,
        Trashed = 5
    }

    /// <summary>
    /// This is an "audit" class to track history.
    /// History opens the door for analysis that the inn keeper can perform to look at past trends
    /// and utilize as input for inventory planning.
    /// This could also help identify how to set Quality levels to help maximize sales
    /// and/or minimize shelf time for inventory.
    /// </summary>
    public class InventoryHistory
    {
        [Key]
        public int HistoryId { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }

        public DateTime InventoryAddedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public InventoryHistoryAction Action { get; set; }
    }
}
