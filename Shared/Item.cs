using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GildedRose_Blazor.Shared
{
    //Defines the Inventory Item data object
    public class Item
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int CategoryId { get; set; }
        public int SellIn { get; set; }
        public int Quality { get; set; }
        public bool QualityAppreciates { get; set; }
        [NotMapped] public string CategoryName { get; set; }
    }
}
