using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace Producers;

public class ProducerRequest
{
    public int Count { get; set; } = 1;
}
