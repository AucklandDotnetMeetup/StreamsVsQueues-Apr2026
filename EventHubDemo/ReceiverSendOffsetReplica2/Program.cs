// See https://aka.ms/new-console-template for more information

using Azure.Messaging.EventHubs;
using Azure.Storage.Blobs;
using Common;

Console.WriteLine($"Start receiving messages from evnehub {ConnectionStrings.EventHubName1} consumer group {ConnectionStrings.ConsumerGroup} ......");

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
var options = new EventProcessorClientOptions
{
    PrefetchCount = 10
};
var processor = new EventProcessorClient(blobContainerClient, ConnectionStrings.ConsumerGroup, ConnectionStrings.EventHubNamespaceConnectionString, ConnectionStrings.EventHubName1, options);

processor.ProcessEventAsync += ProcessorHelpers.ProcessEventHandler;
processor.ProcessErrorAsync += ProcessorHelpers.ProcessErrorHandler;

await processor.StartProcessingAsync(cts.Token);

await tcs.Task;

await processor.StopProcessingAsync();
Console.WriteLine($"{DateTime.UtcNow} Event processor stopped. Exiting application.");