using System.ComponentModel.DataAnnotations.Schema;

namespace GildedRose.Models
{
    public class QualityValue
    {

        public QualityValue()
        {
        }

        public int QualityId { get; set; }
        public int Initial { get; set; }
        public int Current { get; set; }
    }
}