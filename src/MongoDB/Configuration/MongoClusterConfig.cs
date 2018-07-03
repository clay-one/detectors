using MongoDB.Driver;

namespace Detectors.MongoDB.Configuration
{
    public class MongoClusterConfig
    {
        public string Id { get; set; }
        public string ConnectionString { get; set; }

        public MongoClient BuildClient()
        {
            return new MongoClient(ConnectionString);
        }
    }
}