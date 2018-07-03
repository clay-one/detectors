using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
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

        public MongoClient BuildClient(string clusterId)
        {
            var config = GetMongoClusterConfig(clusterId);
            return config?.BuildClient();
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