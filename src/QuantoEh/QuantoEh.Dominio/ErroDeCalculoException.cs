using System;
using System.Runtime.Serialization;

namespace QuantoEh.Dominio
{
    public class ErroDeCalculoException : ApplicationException
    {
        public ErroDeCalculoException()
        {
        }

        public ErroDeCalculoException(string message) : base(message)
        {
        }

        public ErroDeCalculoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ErroDeCalculoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}