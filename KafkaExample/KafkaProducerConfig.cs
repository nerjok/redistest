using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;


namespace KafkaExample;

public class KafkaProducerConfig
{
    public static ProducerConfig GetConfig()
    {
        return new ProducerConfig
        {
            BootstrapServers = "localhost:9092", // Replace with your Kafka broker address
            ClientId = "KafkaExampleProducer",
        };
    }
}
