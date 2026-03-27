using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using System.Text.RegularExpressions;

namespace Common;

public class KafkaConsumer
{
    private readonly ConsumerConfig consumerConfig = new();
    private readonly IConsumer<string, string> consumer;

    public KafkaConsumer()
    {
        consumerConfig.BootstrapServers = "localhost:9092";
        consumerConfig.SecurityProtocol = SecurityProtocol.SaslPlaintext;
        consumerConfig.SaslMechanism = SaslMechanism.Plain;
        consumerConfig.SaslUsername = "$ConnectionString";
        consumerConfig.SaslPassword = ConnectionStrings.EventHubNamespaceConnectionString;
        consumerConfig.GroupId = "$Default";
        consumerConfig.EnableAutoCommit = false;
        consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
        consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
    }

    public void Start(CancellationToken cancellationToken)
    {
        Console.WriteLine($"Start receiving messages from evnehub {ConnectionStrings.EventHubNameForKafka} consumer group $Default ......");
        consumer.Subscribe(ConnectionStrings.EventHubNameForKafka);
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(cancellationToken);
                if (consumeResult != null)
                {
                    Console.WriteLine($"Partiton:{consumeResult.Partition.Value} Offset(SequenceNumber):{consumeResult.Offset.Value} Message received : '{consumeResult.Message.Value}'");
                    consumer.Commit(consumeResult);
                }
            }
            catch (ConsumeException ex)
            {
                Console.WriteLine($"Error consuming message: {ex.Error.Reason}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
        consumer.Close();
    }
}
