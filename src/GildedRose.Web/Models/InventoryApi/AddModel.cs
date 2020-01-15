using System.ComponentModel.DataAnnotations;

namespace GildedRose.Web.Models.InventoryApi
{
    public sealed class AddModel
    {
        [Required]
        public string Category { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Quality { get; set; }

        public int SellIn { get; set; }
    }
}