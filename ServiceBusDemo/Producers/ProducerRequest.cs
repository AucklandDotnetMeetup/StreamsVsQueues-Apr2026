namespace Producers;

public class ProducerRequest
{
    public int Count { get; set; }
}

public class SessionProducerRequest : ProducerRequest
{
    public int Sessions { get; set; }
}
