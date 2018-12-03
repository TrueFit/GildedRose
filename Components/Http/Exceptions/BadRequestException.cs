using System;
using GildedRose.Exceptions;

namespace GildedRose.HttpClient.Exceptions
{
    public class BadRequestException : HaltRequestExecutionException
    {
        public BadRequestException()
         : base()
        {
        }

        public BadRequestException(string message)
            : base(message)
        {
        }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
