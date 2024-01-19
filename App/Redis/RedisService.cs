using StackExchange.Redis;

namespace DotNet.Docker.Redis;

public class RedisService:IRedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase db;

    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        db = _connectionMultiplexer.GetDatabase();
    }

    public async Task<RedisValue> GetStringAsync(string key)
    {
        var value = await db.StringGetAsync(key);
        return value;
    }

    public async Task SetStringAsync(string key, string value)
    {
        await db.StringSetAsync(key, value);
    }
}