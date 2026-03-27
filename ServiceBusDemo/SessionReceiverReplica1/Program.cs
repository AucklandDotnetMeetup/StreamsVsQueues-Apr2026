using Azure.Messaging.ServiceBus;
using Common;
using static Common.ConnectionStrings;


var client = new ServiceBusClient(ServiceBusConnectionString);

var options = new ServiceBusSessionReceiverOptions
{
    PrefetchCount = 10
};
var maxWaitTime = TimeSpan.FromSeconds(5);
Console.WriteLine($"Starting the session receiver {Topic2Name}/Subcription/{SubscriptionName}...");
try
{
    while (true)
    {
        ServiceBusSessionReceiver receiver = await client.AcceptNextSessionAsync(Topic2Name, SubscriptionName, options);
        while (true)
        {
            IReadOnlyList<ServiceBusReceivedMessage> receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages: 10, maxWaitTime);

            if (receivedMessages.Count == 0) break;

            foreach (ServiceBusReceivedMessage message in receivedMessages)
            {
                string body = message.Body.ToString();
                Console.WriteLine($"{Helpers.GetCurrentTime()} {message.SessionId} Received: {body} --- Message ID:{message.MessageId}");
                await Task.Delay(200);
                await receiver.CompleteMessageAsync(message);
            }
        }
        await receiver.DisposeAsync();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
}
finally
{
    await client.DisposeAsync();
}
