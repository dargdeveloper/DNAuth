namespace DotNet.Docker.Redis;

public interface IRedisService
{
    Task<string> GetStringAsync(string key);
    Task SetStringAsync(string key, string value);
}