using Azure.Messaging.ServiceBus;
using Common;

namespace Producers.Services;

public class ServiceBusProducer : IAsyncDisposable
{
    private readonly ServiceBusClient client = new ServiceBusClient(ConnectionStrings.ServiceBusConnectionString);
    public readonly ServiceBusSender Topic1Sender; 
    public readonly ServiceBusSender Topic2Sender; 
    public readonly ServiceBusSender Topic3Sender; 
    public readonly ServiceBusSender Topic4Sender; 

    public ServiceBusProducer()
    {
        Topic1Sender = client.CreateSender(ConnectionStrings.Topic1Name);
        Topic2Sender = client.CreateSender(ConnectionStrings.Topic2Name);
        Topic3Sender = client.CreateSender(ConnectionStrings.Topic3Name);
        Topic4Sender = client.CreateSender(ConnectionStrings.Topic4Name);
    }

    public async ValueTask DisposeAsync()
    {
        await client.DisposeAsync();
    }
}
