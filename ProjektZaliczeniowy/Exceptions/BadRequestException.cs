using System;

namespace ProjektZaliczeniowy.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
                
        }
    }
}
