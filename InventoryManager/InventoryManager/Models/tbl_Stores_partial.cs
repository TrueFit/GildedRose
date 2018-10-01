using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InventoryManager
{
    [MetadataType(typeof(tbl_Stores_partial))]
    public partial class tbl_Stores
    {

    }

    public class tbl_Stores_partial
    {
        [Required]
        public System.Guid Guid { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        [Display(Name = "ZIP Code")]
        public string Zipcode { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public byte[] Logo { get; set; }
        public string LogoName { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public System.DateTime DateCreated { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}