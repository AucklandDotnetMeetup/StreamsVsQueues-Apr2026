using AutoFixture;
using Avro.Specific;
using Common.Schemas;
using Common.Schemas.Avro;
using Common.Schemas.Json;
using Common.Services;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Producers.Controllers;

[ApiController]
[Route("[controller]")]
public class ProducerController : ControllerBase
{
    private readonly ILogger<ProducerController> _logger;
    private readonly ProducerFactory _producerFactory;

    private readonly Fixture fixture = new();

    public ProducerController(ILogger<ProducerController> logger,
        ProducerFactory producerFactory)
    {
        _logger = logger;
        _producerFactory = producerFactory;
    }

    private async Task RunTasks(ProducerRequest producerRequest, Func<Task> func)
    {
        var count = producerRequest.Count < 1 ? 1 : producerRequest.Count;
        var items = Enumerable.Range(1, count).ToList();

        await Parallel.ForEachAsync(items, new ParallelOptions { MaxDegreeOfParallelism = count }, // Optional: limit concurrency
            async (item, cancellationToken) =>
            {
                await func();
            });
        _logger.LogInformation($"Produced {count} messages");
    }

    #region no schema

    [HttpPost("NoSchema")]
    public async Task<IActionResult> CreateMessagesNoSchema([FromBody] ProducerRequest producerRequest)
    {
        await RunTasks(producerRequest, ProduceMessageWithoutSchema);
        return Ok();
    }

    private async Task ProduceMessageWithoutSchema()
    {
        var producer = _producerFactory.GetProducerWithoutSchema();

        var messageKey = Guid.NewGuid().ToString();
        var messageValue = JsonSerializer.Serialize(fixture.Create<UserWithoutSchema>());

        var deliveryResult = await producer.ProduceAsync(TopicNames.TopicWithoutSchema, new Message<string, string>
        {
            Key = messageKey,
            Value = messageValue
        });
    }

    #endregion no schema

    #region avro schema

    [HttpPost("AvroSchema")]
    public async Task<IActionResult> CreateMessagesAvroSchema([FromBody] ProducerRequest producerRequest)
    {
       await RunTasks(producerRequest, ProduceMessageAvroSchema);
       return Ok();
    }

    private async Task ProduceMessageAvroSchema()
    {
        var producer = _producerFactory.GetProducerAvroSchema();

        var messageKey = Guid.NewGuid().ToString();
        var messageValue = fixture.Create<UserAvro>();

        var deliveryResult = await producer.ProduceAsync(TopicNames.TopicWithAvroSchema, new Message<string, ISpecificRecord>
        {
            Key = messageKey,
            Value = messageValue
        });
    }

    #endregion avro schema

    #region json schema

    [HttpPost("JsonSchema")]
    public async Task<IActionResult> CreateMessagesJsonSchema([FromBody] ProducerRequest producerRequest)
    {
       await RunTasks(producerRequest, ProduceMessageJsonSchema);
       return Ok();
    }

    private async Task ProduceMessageJsonSchema()
    {
        var producer = _producerFactory.GetProducerJsonSchema();

        var messageKey = Guid.NewGuid().ToString();
        var messageValue = new UserJson();
        messageValue.Length = fixture.Create<int>();
        messageValue.ID = fixture.Create<string>();
        messageValue.Name = fixture.Create<string>();

        var deliveryResult = await producer.ProduceAsync(TopicNames.TopicWithJsonSchema, new Message<string, UserJson>
        {
            Key = messageKey,
            Value = messageValue
        });
    }

    #endregion json schema

    #region protobuf schema

    [HttpPost("ProtobufSchema")]
    public async Task<IActionResult> CreateMessagesProtobufSchema([FromBody] ProducerRequest producerRequest)
    {
       await RunTasks(producerRequest, ProduceMessageProtobufSchema);
       return Ok();
    }

    private async Task ProduceMessageProtobufSchema()
    {
        var producer = _producerFactory.GetProducerProtobufSchema();

        var messageKey = Guid.NewGuid().ToString();
        var messageValue = fixture.Create<UserProtobuf>();

        var deliveryResult = await producer.ProduceAsync(TopicNames.TopicWithProtobufSchema, new Message<string, UserProtobuf>
        {
            Key = messageKey,
            Value = messageValue
        });
    }

    #endregion protobuf schema

    #region share group

    [HttpPost("ShareGroup")]
    public async Task<IActionResult> CreateMessagesShareGroup([FromBody] ProducerRequest producerRequest)
    {
       await RunTasks(producerRequest, ProduceMessageShareGroup);
       return Ok();
    }

    private async Task ProduceMessageShareGroup()
    {
        var producer = _producerFactory.GetProducerWithoutSchema();

        var messageKey = Guid.NewGuid().ToString();
        var messageValue = JsonSerializer.Serialize(fixture.Create<UserWithoutSchema>());

        var deliveryResult = await producer.ProduceAsync(TopicNames.TopicWithShareGroup, new Message<string, string>
        {
            Key = messageKey,
            Value = messageValue
        });
    }

    #endregion
}