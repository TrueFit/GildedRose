using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GildedRose.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int MaximumQuality { get; set; }
        public int MinimumQuality { get; set; }

        public int DegradationId { get; set; }
        public Degradation Degradation { get; set; }

        public Category()
        {
            MaximumQuality = 50;
            MinimumQuality = 0;
        }
    }
}