using System;

namespace Mediaverse.Domain.Common
{
    public class InformativeException : InvalidOperationException
    {
        public InformativeException(string message, Exception innerException = null) : base(message, innerException)
        { }
    }
}