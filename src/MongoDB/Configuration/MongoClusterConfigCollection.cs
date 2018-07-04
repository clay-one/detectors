using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Detectors.MongoDB.Configuration
{
    public class MongoClusterConfigCollection
    {
        private readonly IConfiguration _configuration;
        
        public MongoClusterConfigCollection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MongoClient GetClient(string clusterId)
        {
            var config = GetMongoClusterConfig(clusterId);
            return config?.BuildClient();
        }

        public IMongoDatabase GetDatabase(string clusterId, string dbName)
        {
            var config = GetMongoClusterConfig(clusterId);
            return config?.GetDatabase(dbName);
        }

        public IMongoCollection<BsonDocument> GetCollection(string clusterId, string dbName, string collectionName)
        {
            var config = GetMongoClusterConfig(clusterId);
            return config?.GetCollection(dbName, collectionName);
        }
        
        public List<MongoClusterConfig> GetAllMongoClusterConfigs()
        {
            return _configuration
                .GetSection("mongodb:clusters")
                .GetChildren()
                .Select(c => c.Get<MongoClusterConfig>())
                .ToList();
        }
        
        public MongoClusterConfig GetMongoClusterConfig(string clusterId)
        {
            return GetAllMongoClusterConfigs().FirstOrDefault(c => c.Id == clusterId);
        }
    }
}