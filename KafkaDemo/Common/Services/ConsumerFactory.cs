using Avro.Specific;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Google.Protobuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Common.Services;

public static class ConsumerFactory
{
    private static readonly CachedSchemaRegistryClient schemaRegistry;

    static ConsumerFactory()
    {
        var config = new SchemaRegistryConfig();
        config.Url = "http://localhost:8081";
        schemaRegistry = new CachedSchemaRegistryClient(config);
    }

    public static IConsumer<string, string> GetConsumerWithoutSchema(string consumerGroup)
    {
        var consumerConfig = GetConsumerConfig(consumerGroup);
        var consumer = new ConsumerBuilder<string, string>(consumerConfig)
                        .Build();
        return consumer;
    }

    public static IConsumer<string, T> GetConsumerWithAvroSchema<T>(string consumerGroup) where T : ISpecificRecord
    {
        var consumerConfig = GetConsumerConfig(consumerGroup);
        var consumer = new ConsumerBuilder<string, T>(consumerConfig)
                        .SetValueDeserializer(new AvroDeserializer<T>(schemaRegistry).AsSyncOverAsync())
                        .Build();
        return consumer;
    }

    public static IConsumer<string, T> GetConsumerWithJsonSchema<T>(string consumerGroup) where T : class
    {
        var consumerConfig = GetConsumerConfig(consumerGroup);
        var consumer = new ConsumerBuilder<string, T>(consumerConfig)
                        .SetValueDeserializer(new JsonDeserializer<T>(schemaRegistry).AsSyncOverAsync())
                        .Build();
        return consumer;
    }

    public static IConsumer<string, T> GetConsumerWithProtobufSchema<T>(string consumerGroup) where T : class, IMessage<T>, new()
    {
        var consumerConfig = GetConsumerConfig(consumerGroup);
        var consumer = new ConsumerBuilder<string, T>(consumerConfig)
                        .SetValueDeserializer(new ProtobufDeserializer<T>(schemaRegistry).AsSyncOverAsync())
                        .Build();
        return consumer;
    }

    private static ConsumerConfig GetConsumerConfig(string consumerGroup)
    {
        var config = new ConsumerConfig();
        config.BootstrapServers = "localhost:9094";
        config.EnableAutoCommit = false;
        config.AutoOffsetReset = AutoOffsetReset.Earliest;
        config.GroupId = consumerGroup;
        return config;
    }
}
