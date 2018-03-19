using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GR.BusinessLogic.Models {
    public class Item  {
        [Key]
        public string Name { get; set; }
        public string Category { get; set; }
        public int SellIn { get; set; }
        public int Quality  { get; set; }
    }
}
