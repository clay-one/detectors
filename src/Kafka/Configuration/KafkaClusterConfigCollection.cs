using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Detectors.Kafka.Configuration
{
    public class KafkaClusterConfigCollection
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
        public KafkaClusterConfigCollection(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        
    }
}