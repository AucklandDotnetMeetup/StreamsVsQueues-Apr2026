using Azure.Messaging.ServiceBus;
using Common;
using static Common.ConnectionStrings;

namespace ReceiverForDeadLetterQueue;

internal static class DeadLetterQueueReceiver
{
    internal static async Task Process(ServiceBusClient client)
    {
        var options = new ServiceBusReceiverOptions()
        {
            PrefetchCount = 10,
            SubQueue = SubQueue.DeadLetter
        };
        var receiver = client.CreateReceiver(Topic4Name, SubscriptionName, options);
        Console.WriteLine($"Starting the receiver {Topic4Name}/Subcription/{SubscriptionName}/DeadLetterQueue...");
        try
        {
            while (true)
            {
                IReadOnlyList<ServiceBusReceivedMessage> receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages: 2);

                if (receivedMessages.Count == 0) continue;

                foreach (ServiceBusReceivedMessage message in receivedMessages)
                {
                    string body = message.Body.ToString();
                    Console.WriteLine($"{Helpers.GetCurrentTime()} Message ID:{message.MessageId} --- Reason: {message.DeadLetterReason} --- {body}");
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
        }
    }
}
