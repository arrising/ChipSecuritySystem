using System;

namespace ChipSecuritySystem
{
    public class ChipValidationException : Exception
    {
        public ChipValidationException(string message) : base(message) { }
    }
}
