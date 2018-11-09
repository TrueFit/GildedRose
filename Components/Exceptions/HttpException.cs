using System;

namespace GildedRose.Exceptions
{
    public abstract class HttpException : Exception
    {
        protected HttpException()
            : base()
        {
        }

        protected HttpException(string message)
            : base(message)
        {
        }

        protected HttpException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
