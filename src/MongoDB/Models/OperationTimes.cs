using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Detectors.MongoDB.Util;

namespace Detectors.MongoDB.Models
{
    [BsonIgnoreExtraElements]
    public class OperationTimes
    {
        #region bson
        [BsonElement("lastCommittedOpTime")]
        [JsonIgnore]
        public OpTime LastCommittedOperation { get; set; }

        [BsonElement("appliedOpTime")]
        [JsonIgnore]
        public OpTime AppliedOperation { get; set; }

        [BsonElement("durableOpTime")]
        [JsonIgnore]
        public OpTime DurableOperation { get; set; }

        [BsonElement("readConcernMajorityOpTime")]
        [JsonIgnore]
        public OpTime ReadMajorityOperation { get; set; }

        [BsonIgnoreExtraElements]
        public class OpTime
        {
            [BsonElement("ts")]
            public BsonTimestamp Timestamp { get; set; }
        }
        #endregion

        #region json
        public DateTime? LastCommittedOperationTime => LastCommittedOperation.Timestamp.ToDateTime();
        public DateTime? AppliedOperationTime => AppliedOperation.Timestamp.ToDateTime();
        public DateTime? DurableOperationTime => DurableOperation?.Timestamp.ToDateTime();
        public DateTime? ReadMajorityOperationTime => ReadMajorityOperation?.Timestamp.ToDateTime();
        #endregion
    }
}