using Azure.Messaging.ServiceBus;
using Common;
using static Common.ConnectionStrings;

namespace ReceiverForDeadLetterQueue;

internal static class SubscriptionReceiver
{
    internal static async Task Process(ServiceBusClient client)
    {
        var options = new ServiceBusReceiverOptions()
        {
            PrefetchCount = 10
        };
        var receiver = client.CreateReceiver(Topic4Name, SubscriptionName, options);
        Console.WriteLine($"Starting the receiver {Topic4Name}/Subcription/{SubscriptionName}...");
        try
        {
            while (true)
            {
                IReadOnlyList<ServiceBusReceivedMessage> receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages: 2);

                if (receivedMessages.Count == 0) continue;

                foreach (ServiceBusReceivedMessage message in receivedMessages)
                {
                    string body = message.Body.ToString();
                    await receiver.DeadLetterMessageAsync(message, "ForDemos", "The message is saved to DLQ for demos.");
                    Console.WriteLine($"{Helpers.GetCurrentTime()} Message ID:{message.MessageId} --- saved to DLQ");
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
        }
    }
}
