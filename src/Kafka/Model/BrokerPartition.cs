using System.Collections.Generic;
using System.Linq;

namespace Detectors.Kafka.Model
{
    public class BrokerPartition
    {
        public int BrokerId { get; set; }

        public List<PartitionInfo> PartitionInfoList { get; set; }

        public PartitionInfo GetPartitionInfo(string topic, int partitionId)
        {
            var result = PartitionInfoList?
                .FirstOrDefault(p => p.Topic == topic
                                     && p.PartitionId == partitionId);

            return result;
        }

        public override string ToString()
        {
            return $"{nameof(BrokerId)} : {BrokerId} {nameof(PartitionInfoList)}.Count : {PartitionInfoList.Count}";
        }
    }
}