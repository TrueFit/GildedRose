using System;
using GildedRose.Exceptions;

namespace GildedRose.Api.Exceptions
{
    public class CriticalStartupException : HaltRequestExecutionException
    {
        public CriticalStartupException()
         : base()
        {
        }

        public CriticalStartupException(string message)
            : base(message)
        {
        }

        public CriticalStartupException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
