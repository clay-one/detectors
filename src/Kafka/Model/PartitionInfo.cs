namespace Detectors.Kafka.Model
{
    public class PartitionInfo
    {
        public string Topic { get; set; }

        public int PartitionId { get; set; }

        public int Leader { get; set; }

        public int[] Replicas { get; set; }

        public int[] ISRs { get; set; }
    }
}