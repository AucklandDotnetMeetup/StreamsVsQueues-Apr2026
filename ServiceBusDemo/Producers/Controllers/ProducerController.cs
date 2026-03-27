using Azure.Messaging.ServiceBus;
using Common;
using Microsoft.AspNetCore.Mvc;
using Producers.Services;

namespace Producers.Controllers;

[ApiController]
[Route("[controller]")]
public class ProducerController : ControllerBase
{
    private readonly ILogger<ProducerController> _logger;
    private readonly ServiceBusProducer _serviceBusProducer;

    public ProducerController(ILogger<ProducerController> logger, ServiceBusProducer serviceBusProducer)
    {
        _logger = logger;
        _serviceBusProducer = serviceBusProducer;
    }

    [HttpPost("single")]
    public async Task<IActionResult> Create()
    {
        var message = new ServiceBusMessage($"ServiceBus Event xxx {DateTime.UtcNow}");
        
        await _serviceBusProducer.Topic1Sender.SendMessageAsync(message);
        _logger.LogInformation($"Sent one message to ServiceBus topic {ConnectionStrings.Topic1Name}");
        return Ok($"Sent 1 message");
    }

    [HttpPost("bybatch")]
    public async Task<IActionResult> CreateByBatch([FromBody] ProducerRequest producerRequest)
    {
        var count = Helpers.GetValueFromMinToMax(producerRequest.Count, 1, 100);

        using ServiceBusMessageBatch messageBatch = await _serviceBusProducer.Topic1Sender.CreateMessageBatchAsync();
        for (int i = 1; i <= count; i++)
        {
            var message = new ServiceBusMessage($"ServiceBus Event {i} {DateTime.UtcNow}");
            if (!messageBatch.TryAddMessage(message))
            {
                throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
            }
        }

        await _serviceBusProducer.Topic1Sender.SendMessagesAsync(messageBatch);
        _logger.LogInformation($"Sent {count} messages to ServiceBus topic {ConnectionStrings.Topic1Name}");
        return Ok($"Sent {count} messages");
    }

    [HttpPost("session")]
    public async Task<IActionResult> CreateBySession([FromBody] ProducerRequest producerRequest)
    {
        var count = Helpers.GetValueFromMinToMax(producerRequest.Count, 1, 100);
        var sessionCount = Helpers.GetValueFromMinToMax(producerRequest.Sessions, 1, 10);

        using ServiceBusMessageBatch messageBatch = await _serviceBusProducer.Topic2Sender.CreateMessageBatchAsync();
        for (int i = 1; i <= count; i++)
        {
            for (int s = 1; s <= sessionCount; s++)
            {
                var sessionId = $"session-{s}";
                var message = new ServiceBusMessage($"ServiceBus Event {i} {sessionId} {DateTime.UtcNow}");
                message.SessionId = sessionId;
                if (!messageBatch.TryAddMessage(message))
                {
                    throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
                }
            }
        }

        await _serviceBusProducer.Topic2Sender.SendMessagesAsync(messageBatch);
        _logger.LogInformation($"Sent {count} messages to ServiceBus topic {ConnectionStrings.Topic2Name}");
        return Ok($"Sent {count} messages to {sessionCount} sessions");
    }

    [HttpPost("schedule")]
    public async Task<IActionResult> CreateBySchedule([FromBody] ProducerRequest producerRequest)
    {
        var count = Helpers.GetValueFromMinToMax(producerRequest.Count, 1, 100);

        var messages = new List<ServiceBusMessage>();
        for (int i = 1; i <= count; i++)
        {
            var message = new ServiceBusMessage($"ServiceBus Event {i} {Helpers.GetCurrentTime()}");
            messages.Add(message);
        }
        var enqueueTime = DateTimeOffset.UtcNow.AddSeconds(30);
        await _serviceBusProducer.Topic3Sender.ScheduleMessagesAsync(messages, enqueueTime);
        _logger.LogInformation($"Sent {count} messages to ServiceBus topic {ConnectionStrings.Topic3Name}");
        return Ok($"Sent {count} messages");
    }

    [HttpPost("dlq")]
    public async Task<IActionResult> CreateByDLQ([FromBody] ProducerRequest producerRequest)
    {
        var count = Helpers.GetValueFromMinToMax(producerRequest.Count, 1, 100);

        using ServiceBusMessageBatch messageBatch = await _serviceBusProducer.Topic4Sender.CreateMessageBatchAsync();
        for (int i = 1; i <= count; i++)
        {
            var message = new ServiceBusMessage($"ServiceBus Event {i} {DateTime.UtcNow}");
            if (!messageBatch.TryAddMessage(message))
            {
                throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
            }
        }

        await _serviceBusProducer.Topic4Sender.SendMessagesAsync(messageBatch);
        _logger.LogInformation($"Sent {count} messages to ServiceBus topic {ConnectionStrings.Topic4Name}");
        return Ok($"Sent {count} messages");
    }

}