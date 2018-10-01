using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryManager
{

    [MetadataType(typeof(tbl_Items_partial))]
    public partial class tbl_Items
    {

    }

    public class tbl_Items_partial
    {
        [Required]
        public System.Guid Guid { get; set; }

        [Required]
        public System.Guid StoreGuid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [Display(Name = "Sell In")]
        public int SellIn { get; set; }

        [Required]
        public int Quality { get; set; }

        [Required]
        public bool Legendary { get; set; }

        [Required]
        [Display(Name = "Better With Age")]
        public bool BetterWithAge { get; set; }

        public byte[] Image { get; set; }

        public string ImageName { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public System.DateTime DateCreated { get; set; }

        [Display(Name = "Date Trashed")]
        public Nullable<System.DateTime> DateTrashed { get; set; }

        [Display(Name = "Date Sold")]
        public System.DateTime DateSold { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}