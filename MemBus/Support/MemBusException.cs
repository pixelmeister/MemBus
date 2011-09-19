using System;
using System.Runtime.Serialization;

namespace MemBus.Support
{
    
    public class MemBusException : Exception
    {
        public MemBusException(string message) : base(message)
        {
        }

        public MemBusException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}