// See https://aka.ms/new-console-template for more information

using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Storage.Blobs;
using Common;

Console.WriteLine($"Start receiving messages from evnehub {ConnectionStrings.EventHubName} consumer group {EventHubConsumerClient.DefaultConsumerGroupName} ......");

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
var processor = new EventProcessorClient(blobContainerClient, EventHubConsumerClient.DefaultConsumerGroupName, ConnectionStrings.EventHubNamespaceConnectionString, ConnectionStrings.EventHubName, options);

processor.ProcessEventAsync += ProcessorHelpers.ProcessEventHandler;
processor.ProcessErrorAsync += ProcessorHelpers.ProcessErrorHandler;

await processor.StartProcessingAsync(cts.Token);

await tcs.Task;

await processor.StopProcessingAsync();
Console.WriteLine($"{DateTime.UtcNow} Event processor stopped. Exiting application.");