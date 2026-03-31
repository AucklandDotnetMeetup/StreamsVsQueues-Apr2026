using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Primitives;
using Azure.Messaging.EventHubs.Processor;
using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace ReceiverWithProcessorBatch;

public class SimpleBatchProcessor : PluggableCheckpointStoreEventProcessor<EventProcessorPartition>
{
    public SimpleBatchProcessor(
        BlobCheckpointStore checkpointStore,
        int maximumBatchSize,
        string consumerGroup,
        string fullyQualifiedNamespace,
        string eventHubName,
        EventProcessorOptions eventProcessorOptions)
        : base(checkpointStore, maximumBatchSize, consumerGroup, fullyQualifiedNamespace, eventHubName, eventProcessorOptions)
    {
    }

    // This method is called to process a batch of events
    protected override async Task OnProcessingEventBatchAsync(
        IEnumerable<EventData> events,
        EventProcessorPartition partition,
        CancellationToken cancellationToken)
    {
        try
        {
            // Log the number of events received
            Console.WriteLine($"Received batch of {events.Count()} events for partition {partition.PartitionId}");

            // Process each event in the batch
            foreach (EventData eventData in events)
            {
                string messageBody = Encoding.UTF8.GetString(eventData.Body.ToArray());
                await Task.Delay(200);
                Console.WriteLine($"{Helpers.GetCurrentTime()} Partiton:{partition.PartitionId} SequenceNumber:{eventData.SequenceNumber} Offset:{eventData.OffsetString} Message received : '{messageBody}'");
            }

            // Checkpoint the partition with the last event in the batch
            if (events.Any())
            {
                var lastEvent = events.Last();
                await UpdateCheckpointAsync(partition.PartitionId, CheckpointPosition.FromEvent(lastEvent), cancellationToken);
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions within the processing logic to prevent the processor from stopping
            Console.Error.WriteLine($"Exception in OnProcessingEventBatchAsync: {ex.Message}");
        }
    }

    // This method is called to handle any errors that occur during processing
    protected override Task OnProcessingErrorAsync(
        Exception exception,
        EventProcessorPartition partition,
        string operationDescription,
        CancellationToken cancellationToken)
    {
        Console.Error.WriteLine($"Exception on partition {partition?.PartitionId} during {operationDescription}: {exception.Message}");
        return Task.CompletedTask;
    }
}
