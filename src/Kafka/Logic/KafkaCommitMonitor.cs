using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Detectors.Kafka.Configuration;
using Microsoft.Extensions.Hosting;

namespace Detectors.Kafka.Logic
{
    public class KafkaCommitMonitor : IHostedService, IDisposable
    {
        private readonly KafkaTopicConsumerWrapper _consumerWrapper;
        private readonly KafkaClusterConfigCollection _configuration;
        private Thread _pollThread;

        public KafkaCommitMonitor(KafkaClusterConfigCollection configuration)
        {
            _configuration = configuration;
            _consumerWrapper = _configuration.BuildTopicConsumerWrapper("s1", "__consumer_offsets",
                "consumer-" + Guid.NewGuid().ToString("N"));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("START :)");
            _consumerWrapper.Consumer.Value.OnMessage += (sender, message) =>
            {
                Console.WriteLine("---------------------------------------------------------------");
                KafkaGroupMetadataManager.ReadMessage(message.Key, message.Value);
//                Console.WriteLine($"@{message.Timestamp.UtcDateTime:O}: {Convert.ToBase64String(message.Key ?? new byte[0])} = {Convert.ToBase64String(message.Value ?? new byte[0])}");
            };
            _consumerWrapper.Consumer.Value.Subscribe("__consumer_offsets");

            _pollThread = new Thread(() =>
            {
                while (true)
                {
                    _consumerWrapper.Consumer.Value.Poll(100);
                }
            });
            
            _pollThread.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("STOP :(");
            _consumerWrapper.Consumer.Value.Unsubscribe();
            _pollThread.Abort();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _consumerWrapper?.Dispose();
        }
    }
}