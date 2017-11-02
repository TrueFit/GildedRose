using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Utilities
{
    public static class DateTimeUtility
    {
        /// <summary>
        /// Return the earlier of two dates.
        /// </summary>
        public static DateTime Min(DateTime dt1, DateTime dt2)
            => dt1 < dt2 ? dt1 : dt2;

        /// <summary>
        /// Return the later of two dates.
        /// </summary>
        public static DateTime Max(DateTime dt1, DateTime dt2)
            => dt1 > dt2 ? dt1 : dt2;

        /// <summary>
        /// Return the earlier of two dates with null being considered the lowest possible value.
        /// </summary>
        public static DateTime? Min(DateTime? dt1, DateTime? dt2)
            => dt1 == null || dt2 == null ? (DateTime?)null : Min(dt1.Value, dt2.Value);

        /// <summary>
        /// Return the later of two dates with null being considered the highest possible value.
        /// </summary>
        public static DateTime? Max(DateTime? dt1, DateTime? dt2)
            => dt1 == null || dt2 == null ? (DateTime?)null : Max(dt1.Value, dt2.Value);
    }
}
