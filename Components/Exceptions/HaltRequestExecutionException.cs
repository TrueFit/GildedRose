using System;

namespace GildedRose.Exceptions
{
    public abstract class HaltRequestExecutionException : Exception
    {
        protected HaltRequestExecutionException()
            : base()
        {
        }

        protected HaltRequestExecutionException(string message)
            : base(message)
        {
        }

        protected HaltRequestExecutionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
