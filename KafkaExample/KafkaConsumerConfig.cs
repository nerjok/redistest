using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;


namespace KafkaExample
{
    public class KafkaConsumerConfig
    {
        public static ConsumerConfig GetConfig()
        {
            return new ConsumerConfig
            {
                BootstrapServers = "localhost:9092", // Replace with your Kafka broker address
                GroupId = "KafkaExampleConsumer",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
            };
        }
    }
}