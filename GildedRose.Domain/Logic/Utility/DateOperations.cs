using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Domain.Logic.Utility
{
    public static class DateOperations
    {
        public static int GetDaysBetweenDates(DateTime startDate, DateTime endDate)
        {
            return (startDate - endDate).Days;
        }
    }
}
