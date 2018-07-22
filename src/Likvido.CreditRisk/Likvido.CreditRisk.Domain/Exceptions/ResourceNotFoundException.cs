using System;

namespace Likvido.CreditRisk.Domain.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException() : base() { }

        public ResourceNotFoundException(string message) : base(message) { }
    }
}
