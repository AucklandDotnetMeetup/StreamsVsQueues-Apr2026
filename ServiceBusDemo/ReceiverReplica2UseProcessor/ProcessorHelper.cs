using Azure.Messaging.ServiceBus;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiverReplica2UseProcessor;

internal static class ProcessorHelper
{
    public static async Task MessageHandler(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        string body = message.Body.ToString();
        Console.WriteLine($"{Helpers.GetCurrentTime()} Received: {body} ---SequenceNumber:{message.SequenceNumber} --- Message ID:{message.MessageId}");
        await Task.Delay(200);
        await args.CompleteMessageAsync(args.Message);
    }

    public static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine($"Error Source: {args.ErrorSource}");
        Console.WriteLine($"Exception: {args.Exception.Message}");
        return Task.CompletedTask;
    }
}
