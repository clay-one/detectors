using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Detectors.MongoDB.Models
{
    [BsonIgnoreExtraElements]
    public class ReplicaSetStatus
    {
        [BsonElement("set")]
        [BsonIgnoreIfNull]
        public string SetName { get; set; }

        [BsonElement("date")]
        [BsonIgnoreIfNull]
        public DateTime ServerTime { get; set; }

        [BsonElement("myState")]
        [BsonIgnoreIfNull]
        public MemberState ServerState { get; set; }

        [BsonIgnore]
        public string ServerStateName => ServerState.ToString().ToUpperInvariant();

        [BsonElement("term")]
        [BsonIgnoreIfNull]
        public int Term { get; set; }

        [BsonElement("heartbeatIntervalMillis")]
        [BsonIgnoreIfNull]
        public long HeartbeatIntervalMilliseconds { get; set; }

        [BsonElement("optimes")]
        [BsonIgnoreIfNull]
        public OperationTimes OperationTimes { get; set; }

        [BsonElement("members")]
        [BsonIgnoreIfNull]
        public ReplicaSetMember[] Members { get; set; }

        [BsonElement("ok")]
        [JsonIgnore]
        public bool Ok { get; set; }
    }
}