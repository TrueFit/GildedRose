using GildedRose.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GildedRose.Domain.Models
{
    public class CategoryValue
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int MaximumQuality { get; set; }
        public int MinimumQuality { get; set; }

        public int DegradationId { get; set; }
        public DegradationValue Degradation { get; set; }

        public CategoryValue()
        {
            MaximumQuality = 50;
            MinimumQuality = 0;
        }
    }
}