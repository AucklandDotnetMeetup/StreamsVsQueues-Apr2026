using Azure.Messaging.ServiceBus;
using ReceiverReplica2UseProcessor;
using static Common.ConnectionStrings;

var client = new ServiceBusClient(ServiceBusConnectionString);
string queueName = $"{Topic2Name}/Subscriptions/{SubscriptionName}";

ServiceBusSessionProcessor processor = client.CreateSessionProcessor(queueName, new ServiceBusSessionProcessorOptions
{
    PrefetchCount = 10,
    AutoCompleteMessages = false,
    MaxConcurrentSessions = 1,
    SessionIdleTimeout = TimeSpan.FromSeconds(5)
});

try
{
    // 3. Add the handlers
    processor.ProcessMessageAsync += ProcessorHelper.MessageHandler;
    processor.ProcessErrorAsync += ProcessorHelper.ErrorHandler;

    // 4. Start processing
    Console.WriteLine($"Starting the session processor {queueName}...");
    await processor.StartProcessingAsync();

    Console.ReadKey();

    await processor.StopProcessingAsync();
}
finally
{
    await processor.DisposeAsync();
    await client.DisposeAsync();
}