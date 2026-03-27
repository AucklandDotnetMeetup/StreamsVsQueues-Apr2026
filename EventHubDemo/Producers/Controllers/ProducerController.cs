using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Common;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Producers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProducerController : ControllerBase
    {
        private readonly ILogger<ProducerController> _logger;
        private readonly EventHubProducerClient _producerClient;
        private readonly KafkaProducer _kafkaProducer;

        public ProducerController(ILogger<ProducerController> logger,
            EventHubProducerClient producerClient,
            KafkaProducer kafkaProducer)
        {
            _logger = logger;
            _producerClient = producerClient;
            _kafkaProducer = kafkaProducer;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProducerRequest producerRequest)
        {
            var count = producerRequest.Count < 1 ? 1 : producerRequest.Count;
            var batchOptions = new CreateBatchOptions
            {
                PartitionKey = "test-message-key"
            };
            using EventDataBatch eventBatch = await _producerClient.CreateBatchAsync(batchOptions);
            for (int i = 1; i <= count; i++)
            {
                var message = new EventData(Encoding.UTF8.GetBytes($"Event {i} {DateTime.UtcNow}"));

                if (!eventBatch.TryAdd(message))
                {
                    // if it is too large for the batch
                    throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
                }
            }

            await _producerClient.SendAsync(eventBatch);
            _logger.LogInformation($"Sent {count} messages to event hub");
            return Ok($"Sent {count} messages");
        }

        [HttpPost("bykafka")]
        public async Task<IActionResult> CreateThroughKafka([FromBody] ProducerRequest producerRequest)
        { 
            var count = producerRequest.Count < 1 ? 1 : producerRequest.Count;
            var items = Enumerable.Range(1, count).ToList();

            await Parallel.ForEachAsync(items, new ParallelOptions { MaxDegreeOfParallelism = count },
                async (i, cancellationToken) =>
                {
                    var messageKey = "test-message-key";
                    var messageValue = $"Event {i} {DateTime.UtcNow}";
                    await _kafkaProducer.ProduceAsync(ConnectionStrings.EventHubNameForKafka, new Confluent.Kafka.Message<string, string> { Key = messageKey, Value = messageValue });
                });
            _logger.LogInformation($"Sent {count} messages to event hub using kafka producer");
            return Ok($"Sent {count} messages through Kafka");
        }
    }
}
