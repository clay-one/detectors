namespace Detectors.Kafka.Model
{
    public class NotSyncReplica
    {
        public int BrokerId1 { get; set; }

        public int Leader1 { get; set; }

        public string Replicas1 { get; set; }

        public string ISRs1 { get; set; }

        public string Topic { get; set; }

        public int PartitionId { get; set; }

        public int BrokerId2 { get; set; }

        public int Leader2 { get; set; }

        public string Replicas2 { get; set; }

        public string ISRs2 { get; set; }
    }
}