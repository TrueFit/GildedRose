using System.ComponentModel.DataAnnotations.Schema;

namespace GildedRose.Models
{
    public class DegradationValue
    {
        public int DegradationId { get; set; }
        /// <summary>
        /// The amount of days prior to Sell In where the Degradation rate begins to change
        /// </summary>
        private int _threshold;
        public int Threshold {
            get
            {
                return _threshold;
            }
            set
            {
                if (value < 0)
                    throw new System.Exception($"Degradation Threshold must be greater than or equal to 0, {value} is invalid.");
                _threshold = value;
            }
        }
        /// <summary>
        /// The interval (in days) in which the value of 
        /// </summary>
        private int _interval;
        public int Interval
        {
            get {
                return _interval;
            }
            set
            {
                if (value < 0)
                    throw new System.Exception($"Degradation Interval must be greater than or equal to 0, {value} is invalid.");
                _interval = value;
            }
        }
        /// <summary>
        /// The number of quality points that will be added (or reduced) with every interval that is reached. 
        /// </summary>
        public int Rate { get; set; }

        /// <summary>
        /// Indicates that immediately following expiration, the value of a product is 0
        /// </summary>
        public bool HasNoValuePastExpiration { get; set; }

        public DegradationValue()
        {
            Threshold = 0;
            Interval = 1;
            Rate = 1;
            HasNoValuePastExpiration = false;
        }
    }
}