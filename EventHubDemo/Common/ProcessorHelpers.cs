using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Common;
using System.Text;

namespace Common;

public static class ProcessorHelpers
{
    public static async Task CreateCheckpointBlob()
    {
        BlobServiceClient blobServiceClient = new BlobServiceClient(ConnectionStrings.BlobServiceConnectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(ConnectionStrings.CheckpointBlobContainer);

        await containerClient.CreateIfNotExistsAsync();
    }

    public static async Task ProcessEventHandler(ProcessEventArgs eventArgs)
    {
        string messageBody = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());
        Console.WriteLine($"Partiton:{eventArgs.Partition.PartitionId} SequenceNumber:{eventArgs.Data.SequenceNumber} Offset:{eventArgs.Data.OffsetString} Message received : '{messageBody}'");
        await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
    }

    public static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
    {
        // Write details about the error to the console window
        Console.WriteLine($"\tPartition '{eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
        Console.WriteLine(eventArgs.Exception.Message);
        return Task.CompletedTask;
    }
}