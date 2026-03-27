using Azure.Messaging.ServiceBus;
using ReceiverReplica2UseProcessor;
using static Common.ConnectionStrings;

var client = new ServiceBusClient(ServiceBusConnectionString);
string queueName = $"{Topic1Name}/Subscriptions/{SubscriptionName}";

ServiceBusProcessor processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions
{
    PrefetchCount = 10,
    AutoCompleteMessages = false,
    MaxConcurrentCalls = 2
});

try
{
    // 3. Add the handlers
    processor.ProcessMessageAsync += ProcessorHelper.MessageHandler;
    processor.ProcessErrorAsync += ProcessorHelper.ErrorHandler;

    // 4. Start processing
    Console.WriteLine($"Starting the processor {queueName}...");
    await processor.StartProcessingAsync();

    Console.ReadKey();

    await processor.StopProcessingAsync();
}
finally
{
    await processor.DisposeAsync();
    await client.DisposeAsync();
}