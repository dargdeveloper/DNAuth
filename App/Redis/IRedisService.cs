using System.IdentityModel.Tokens.Jwt;
using DotNet.Docker.Models;
using StackExchange.Redis;

namespace DotNet.Docker.Redis;

public interface IRedisService
{
    Task<RedisValue> GetStringAsync(string key);
    Task SetStringAsync(string key, string value);
    SSOUser GetUser(string value);
}