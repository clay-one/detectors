using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Detectors.MongoDB.Models
{
    [BsonIgnoreExtraElements]
    public class ReplicaSetMember
    {
        [BsonElement("_id")]
        public long Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("self")]
        public bool IsMe { get; set; }

        [BsonElement("health")]
        public bool IsHealthy { get; set; }

        [BsonElement("state")]
        public MemberState State { get; set; }

        [BsonElement("stateStr")]
        public string StateName { get; set; }

        [BsonElement("uptime")]
        public long UptimeSeconds { get; set; }

        [BsonElement("optimeDate")]
        public DateTime LastOperationTime { get; set; }

        [BsonElement("optimeDurableDate")]
        public DateTime DurableOperationTime { get; set; }

        [BsonElement("lastHeartbeat")]
        public DateTime LastHearthbeat { get; set; }

        [BsonElement("pingMs")]
        public long Ping { get; set; }
        
        [BsonElement("electionDate")]
        public DateTime ElectionTime { get; set; }

        [BsonElement("configVersion")]
        public long ConfigVersion { get; set; }
    }
}