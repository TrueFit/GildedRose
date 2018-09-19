using GildedRose.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GildedRose.Models
{
    public class ProductAgingViewModel
    {
        [Required(ErrorMessage = "As-of Aging date is required")]
        [Display(Name = "As-of Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AsOfDate { get; set; }

        public Guid? ProductId { get; set; }

        public bool ZeroOnly { get; set; }

        public IList<ProductAgingList> ProductAges { get; set; }
        public IList<ProductList> Products { get; set; }

    }

    public class ProductList
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
    }
}