using System.Text.RegularExpressions;
using DotNet.Docker.Helpers;
using DotNet.Docker.SSO;
using StackExchange.Redis;
using System.Text.Json;

namespace DotNet.Docker.Redis;

public class RedisService:IRedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase db;
    private readonly IHelper helper;

    public RedisService(
        IConnectionMultiplexer connectionMultiplexer,
        IHelper helper
        )
    {
        _connectionMultiplexer = connectionMultiplexer;
        db = _connectionMultiplexer.GetDatabase();
        this.helper = helper;
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

    public SSOUser GetUser(string value)
    {
        string pattern = @"^[a-zA-Z0-9]+:\d+:";

        // Dividir la cadena, pero solo en la primera coincidencia
        string[] partes = Regex.Split(value.ToString(), pattern);
        // string jsonString = "@" + partes[1];

        string nuevo = helper.EscapeJsonString(partes[1].Replace(";",""));
        // using JsonDocument doc = JsonDocument.Parse(nuevo);

        // var objeto = JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
        
        SSOUser sso = JsonSerializer.Deserialize<SSOUser>(nuevo);

        return sso;
    }
}