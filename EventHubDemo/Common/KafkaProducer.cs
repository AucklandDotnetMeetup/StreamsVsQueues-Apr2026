using Confluent.Kafka;

namespace Common;

public class KafkaProducer : IDisposable
{
    private readonly ProducerConfig producerConfig = new();
    private readonly IProducer<string, string> producer;

    public KafkaProducer()
    {
        producerConfig.BootstrapServers = "localhost:9092";
        producerConfig.SecurityProtocol = SecurityProtocol.SaslPlaintext;
        producerConfig.SaslMechanism = SaslMechanism.Plain;
        producerConfig.SaslUsername = "$ConnectionString";
        producerConfig.SaslPassword = ConnectionStrings.EventHubNamespaceConnectionString;
        producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }

    public void Dispose()
    {
        producer.Dispose();
    }

    public async Task ProduceAsync(string topic, Message<string, string> message)
    {
        try
        {
            var deliveryResult = await producer.ProduceAsync(topic, message);
        }
        catch (ProduceException<string, string> ex)
        {
            Console.WriteLine($"Failed to deliver message: {ex.Error.Reason}");
        }
    }
}