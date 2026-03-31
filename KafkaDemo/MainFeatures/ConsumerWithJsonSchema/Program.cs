using Common.Schemas.Json;
using Common.Services;

var topic = TopicNames.TopicWithJsonSchema;
var group = ConsumerGroups.ConsumerGroup;

var consumer = ConsumerFactory.GetConsumerWithJsonSchema<UserJson>(group);

consumer.Subscribe(topic);
Console.WriteLine($"start consumer for topic {topic} group {group}");

while (true)
{
    try
    {
        var consumeResult = consumer.Consume();
        if (consumeResult == null || consumeResult.Message == null || consumeResult.Message.Value == null)
            continue;

        var result = consumeResult.Message.Value;

        consumer.Commit();
        Console.WriteLine($"Key: {consumeResult.Message.Key} topic partition: {consumeResult.TopicPartition} partition: {consumeResult.Partition} offset: {consumeResult.Offset} Value: {result?.ID} {result?.Name} {result?.Length}");
    }
    catch (Exception e)
    {
        Console.WriteLine($"Consume error: {e}");
    }
}