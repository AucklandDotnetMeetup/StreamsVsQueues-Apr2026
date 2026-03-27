using Avro.Specific;
using Common.Schemas.Json;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using System.Collections.Concurrent;

namespace Common.Services;

public class ProducerFactory : IDisposable
{
    private readonly CachedSchemaRegistryClient schemaRegistry;
    private readonly ConcurrentDictionary<KafkaSchemaTypes, IClient> producers = new();
    private readonly ProducerConfig producerConfig = new();

    public ProducerFactory()
    {
        var config = new SchemaRegistryConfig();
        config.Url = "http://localhost:8081";
        schemaRegistry = new CachedSchemaRegistryClient(config);

        producerConfig.BootstrapServers = "localhost:9094";
    }

    public IProducer<string, string> GetProducerWithoutSchema()
    {
        var result = producers.GetOrAdd(KafkaSchemaTypes.None, key =>
        {
            var producer = new ProducerBuilder<string, string>(producerConfig).Build();
            return producer;
        });
        return (IProducer<string, string>)result;
    }

    public IProducer<string, ISpecificRecord> GetProducerAvroSchema()
    {
        var result = producers.GetOrAdd(KafkaSchemaTypes.Avro, key =>
        {
            var producer = new ProducerBuilder<string, ISpecificRecord>(producerConfig)
            .SetValueSerializer(new AvroSerializer<ISpecificRecord>(schemaRegistry))
            .Build();
            return producer;
        });
        return (IProducer<string, ISpecificRecord>)result;
    }

    public IProducer<string, UserJson> GetProducerJsonSchema()
    {
        var result = producers.GetOrAdd(KafkaSchemaTypes.Json, key =>
        {
            var producer = new ProducerBuilder<string, UserJson>(producerConfig)
            .SetValueSerializer(new JsonSerializer<UserJson>(schemaRegistry))
            .Build();
            return producer;
        });
        return (IProducer<string, UserJson>)result;
    }

    public IProducer<string, UserProtobuf> GetProducerProtobufSchema()
    {
        var result = producers.GetOrAdd(KafkaSchemaTypes.Protobuf, key =>
        {
            var producer = new ProducerBuilder<string, UserProtobuf>(producerConfig)
            .SetValueSerializer(new ProtobufSerializer<UserProtobuf>(schemaRegistry))
            .Build();
            return producer;
        });
        return (IProducer<string, UserProtobuf>)result;
    }

    public void Dispose()
    {
        foreach (var item in producers.Values)
        {
            item.Dispose();
        }
        schemaRegistry?.Dispose();
    }
}
