using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GildedRose.Models
{
    public class Quality
    {

        public Quality()
        {
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QualityId { get; set; }
        public int Initial { get; set; }
        public int Current { get; set; }
    }
}