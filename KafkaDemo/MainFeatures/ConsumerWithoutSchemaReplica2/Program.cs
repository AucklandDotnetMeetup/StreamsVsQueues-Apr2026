using Common.Schemas;
using Common.Services;
using System.Text.Json;

var topic = TopicNames.TopicWithoutSchema;
var group = ConsumerGroups.ConsumerGroup;

var consumer = ConsumerFactory.GetConsumerWithoutSchema(group);

consumer.Subscribe(topic);
Console.WriteLine($"start consumer for topic {topic} group {group}");

while (true)
{
    try
    {
        var consumeResult = consumer.Consume();
        if (consumeResult == null || consumeResult.Message == null || consumeResult.Message.Value == null)
            continue;

        var result = JsonSerializer.Deserialize<UserWithoutSchema>(consumeResult.Message.Value);

        consumer.Commit();
        await Task.Delay(100);
        Console.WriteLine($"Key: {consumeResult.Message.Key} topic partition: {consumeResult.TopicPartition} partition: {consumeResult.Partition} offset: {consumeResult.Offset} Value: {result?.ID} {result?.Name} {result?.Length}");
    }
    catch (Exception e)
    {
        Console.WriteLine($"Consume error: {e}");
    }
}