using Azure.Messaging.ServiceBus;
using ReceiverForDeadLetterQueue;
using static Common.ConnectionStrings;

var client = new ServiceBusClient(ServiceBusConnectionString);
var task1 = SubscriptionReceiver.Process(client);
var task2 = DeadLetterQueueReceiver.Process(client);

await Task.WhenAll(task1, task2);
await client.DisposeAsync();