using Azure.Messaging.EventHubs.Consumer;
using Common;
using System.Text;

Console.WriteLine($"Start receiving messages from evnehub {ConnectionStrings.EventHubName1} consumer group {EventHubConsumerClient.DefaultConsumerGroupName} ......");

var consumer = new EventHubConsumerClient(EventHubConsumerClient.DefaultConsumerGroupName, ConnectionStrings.EventHubNamespaceConnectionString, ConnectionStrings.EventHubName1);

try
{
    await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(new ReadEventOptions { MaximumWaitTime = TimeSpan.FromSeconds(2) }))
    {
        if (partitionEvent.Data != null)
        {
            string messageBody = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());
            Console.WriteLine($"Partiton:{partitionEvent.Partition.PartitionId} SequenceNumber:{partitionEvent.Data.SequenceNumber} Offset:{partitionEvent.Data.OffsetString} Message received : '{messageBody}'");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error receiving events: {ex.Message}");
}
finally
{
    await consumer.CloseAsync();
}