using System;
using Confluent.Kafka;
using KafkaExample;
// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");


class Program
{
    static void Main()
    {
        var config = KafkaProducerConfig.GetConfig();

        using var producer = new ProducerBuilder<Null, string>(config).Build();
        string topic = "my-topic"; // Replace with your topic name
        string message = "Hello, Kafka!";
        var deliveryReport = producer.ProduceAsync(topic, new Message<Null, string> { Value = message }).Result;
        Console.WriteLine($"Produced message to {deliveryReport.Topic} partition {deliveryReport.Partition} @ offset {deliveryReport.Offset}");

        producer.ProduceAsync(topic, new Message<Null, string> { Value = " two" });
        // Consumer

        var configC = KafkaConsumerConfig.GetConfig();
        using var consumer = new ConsumerBuilder<Ignore, string>(configC).Build();

        consumer.Subscribe(topic);
        while (true)
        {
            try
            {
                var consumeResult = consumer.Consume();
                Console.WriteLine($"Received message: {consumeResult.Message.Value} Offset: {consumeResult.Offset} topic: {consumeResult.Topic}");

                // Process the message here
                consumer.Commit(consumeResult);
                Thread.Sleep(10000);
                producer.ProduceAsync(topic, new Message<Null, string> { Value = $"{consumeResult.Message.Value} +  two" });
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Error consuming message: {e.Error.Reason}");
            }
        }
    }
}