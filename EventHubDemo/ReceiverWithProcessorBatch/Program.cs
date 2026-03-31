using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Primitives;
using Azure.Storage.Blobs;
using Common;
using ReceiverWithProcessorBatch;

Console.WriteLine($"Start receiving messages from evnehub {ConnectionStrings.EventHubName3} consumer group {ConnectionStrings.ConsumerGroup} ......");

var tcs = new TaskCompletionSource<bool>();
var cts = new CancellationTokenSource();

Console.CancelKeyPress += (sender, eventArgs) =>
{
    Console.WriteLine($"{DateTime.UtcNow} Ctrl+C pressed. Exiting...");
    eventArgs.Cancel = true; // Prevent immediate termination
    tcs.TrySetResult(true);  // Signal to continue
    cts.Cancel(); // Cancel any ongoing operations
};

await ProcessorHelpers.CreateCheckpointBlob();

BlobContainerClient blobContainerClient = new BlobContainerClient(ConnectionStrings.BlobServiceConnectionString, ConnectionStrings.CheckpointBlobContainer);
var checkpointStore = new BlobCheckpointStore(blobContainerClient);
var maximumBatchSize = 5;
var eventHubName = ConnectionStrings.EventHubName3;

EventProcessorOptions eventProcessorOptions = new()
{
    PrefetchCount = 10
};

// Initialize the custom batch processor
var processor = new SimpleBatchProcessor(
    checkpointStore,
    maximumBatchSize,
    ConnectionStrings.ConsumerGroup,
    ConnectionStrings.EventHubNamespaceConnectionString,
    eventHubName,
    eventProcessorOptions);

await processor.StartProcessingAsync(cts.Token);

await tcs.Task;

await processor.StopProcessingAsync();

Console.WriteLine($"{DateTime.UtcNow} Event processor stopped. Exiting application.");