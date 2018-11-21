using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace GildedRose.Model
{
    public class StoreItemDto : IStoreItemDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int SellIn { get; set; }
        public int Quality { get; set; }
    }
}
