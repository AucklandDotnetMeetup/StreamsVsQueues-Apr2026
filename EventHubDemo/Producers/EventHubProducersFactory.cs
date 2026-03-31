using Azure.Messaging.EventHubs.Producer;
using Common;
using System.Collections.Concurrent;

namespace Producers;

public class EventHubProducersFactory
{
    private readonly ConcurrentDictionary<string, EventHubProducerClient> _producers = new();

    public EventHubProducerClient GetProducer(string eventhubName)
    {
        return _producers.GetOrAdd(eventhubName, (eh) =>
        {
           return new EventHubProducerClient(ConnectionStrings.EventHubNamespaceConnectionString, eh);
        });
    }
}
