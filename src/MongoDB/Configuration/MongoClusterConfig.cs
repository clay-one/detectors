using System.Linq;
using MongoDB.Bson;
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

        public MongoClient BuildDirecetClient()
        {
            var urlBuilder = new MongoUrlBuilder(ConnectionString);
            urlBuilder.ConnectionMode = ConnectionMode.Direct;
            urlBuilder.Server = urlBuilder.Servers.First();

            return new MongoClient(urlBuilder.ToMongoUrl());
        }

        public IMongoDatabase GetDatabase(string dbName)
        {
            var client = BuildClient();
            return client?.GetDatabase(dbName);
        }

        public IMongoCollection<BsonDocument> GetCollection(string dbName, string collectionName)
        {
            var database = GetDatabase(dbName);
            return database.GetCollection<BsonDocument>(collectionName);
        }
    }
}