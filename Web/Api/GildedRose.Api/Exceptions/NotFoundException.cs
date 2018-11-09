﻿using GildedRose.Exceptions;
using System;

namespace GildedRose.Api.Exceptions
{
    public class NotFoundException : HaltRequestExecutionException
    {
        public NotFoundException()
         : base()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
