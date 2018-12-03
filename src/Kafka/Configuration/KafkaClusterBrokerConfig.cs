namespace Detectors.Kafka.Configuration
{
    public class KafkaClusterBrokerConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public override string ToString()
        {
            return $"{Host}:{Port}";
        }
    }
}