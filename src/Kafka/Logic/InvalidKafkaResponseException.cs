using System;

namespace Detectors.Kafka.Logic
{
    public class InvalidKafkaResponseException : Exception
    {
        public InvalidKafkaResponseException()
        {
        }

        public InvalidKafkaResponseException(string message) : base(message)
        {
        }

        public InvalidKafkaResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}