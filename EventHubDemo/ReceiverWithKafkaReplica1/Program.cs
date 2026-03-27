// See https://aka.ms/new-console-template for more information
using Common;

CancellationTokenSource cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArgs) =>
{
    Console.WriteLine($"{DateTime.UtcNow} Ctrl+C pressed. Exiting...");
    eventArgs.Cancel = true; // Prevent immediate termination
    cts.Cancel(); // Cancel any ongoing operations
};

KafkaConsumer consumer = new();
consumer.Start(cts.Token);
Console.WriteLine("Finished consuming messages. Exiting application.");