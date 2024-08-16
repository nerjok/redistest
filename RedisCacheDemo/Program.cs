using System.Text.Json;
using StackExchange.Redis;
using static StackExchange.Redis.RedisChannel;

class Program
{
    static void Main(string[] args)
    {
        var configuration = ConfigurationOptions.Parse("localhost:6379");
        var redisConnection = ConnectionMultiplexer.Connect(configuration);

        var redisCache = redisConnection.GetDatabase();

        var json = JsonSerializer.Serialize("serializeDeserialize");
        var subscriber = redisConnection.GetSubscriber();

        subscriber.SubscribeAsync(new RedisChannel("channelName", PatternMode.Literal), async (channel, message) =>
        {
            var value = JsonSerializer.Deserialize<string>(message);
            Thread.Sleep(5000);
            Console.WriteLine("[[ KUKU {0}]]", value);
            var json = JsonSerializer.Serialize("serializeDeserialize" + value);
            subscriber.PublishAsync(new RedisChannel("channelName", PatternMode.Literal), json, CommandFlags.FireAndForget);
        });
        subscriber.PublishAsync(new RedisChannel("channelName", PatternMode.Literal), json, CommandFlags.FireAndForget);

        Console.WriteLine("Fetching data with caching:");

        var cachedData = GetDataWithCaching(redisCache);
        Console.WriteLine($"Result: {cachedData}");

        Console.WriteLine("Fetching data without caching:");
        var uncachedData = GetDataFromDatabase();

        Console.WriteLine($"Result: {uncachedData}");

        // redisConnection.Close(); //It is important to close the connection

        while (true)
        {
            Thread.Sleep(1000); // Sleep to prevent high CPU usage
        }
    }

    static string GetDataFromDatabase()
    {
        // Simulate fetching data from the database
        // Replace this with your actual database fetching logic
        Thread.Sleep(2000); // Simulating latency

        return "Data from database";
    }

    static string GetDataWithCaching(IDatabase redisCache)
    {
        string cachedData = redisCache.StringGet("cachedData");
        if (string.IsNullOrEmpty(cachedData))
        {
            cachedData = GetDataFromDatabase();
            redisCache.StringSet("cachedData", cachedData, TimeSpan.FromMinutes(10));
        }
        return cachedData;
    }
}