using StackExchange.Redis;

namespace DotNet.Docker.Redis;

public interface IRedisService
{
    Task<RedisValue> GetStringAsync(string key);
    Task SetStringAsync(string key, string value);
}