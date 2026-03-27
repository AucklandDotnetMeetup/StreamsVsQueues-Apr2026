using Azure.Messaging.ServiceBus;
using Common;
using static Common.ConnectionStrings;


var client = new ServiceBusClient(ServiceBusConnectionString);

var options = new ServiceBusReceiverOptions()
{
    PrefetchCount = 10
};
var receiver = client.CreateReceiver(Topic3Name, SubscriptionName, options);
Console.WriteLine($"Starting the receiver {Topic3Name}/Subcription/{SubscriptionName}...");
try
{
    while (true)
    {
        IReadOnlyList<ServiceBusReceivedMessage> receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages: 2);

        if(receivedMessages.Count == 0) continue;

        foreach (ServiceBusReceivedMessage message in receivedMessages)
        {
            string body = message.Body.ToString();
            Console.WriteLine($"{Helpers.GetCurrentTime()} Received: {body} --- Message ID:{message.MessageId} ScheduledEnqueueTime:{message.ScheduledEnqueueTime}");
            await Task.Delay(200);
            await receiver.CompleteMessageAsync(message);
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
}
finally
{
    await receiver.CloseAsync();
    await client.DisposeAsync();
}
