using System.Text.Json;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Persistence.Services.Helpers;
using StackExchange.Redis;

namespace SpeechGPT.Persistence.Services;

public class RedisCache : IRedisCache
{
    private IDatabase _db;
    private readonly ConnectionHelper _connectionHelper;
    
    public RedisCache(ConnectionHelper connectionHelper)
    {
        _connectionHelper = connectionHelper;
        ConfigureRedis();
    }
    
    private void ConfigureRedis()
    {
        _db = _connectionHelper.Connection.GetDatabase();
    }

    public async Task<T> GetCacheData<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        if (!string.IsNullOrEmpty(value))
        {
            return JsonSerializer.Deserialize<T>(value);
        }
        return default;
    }

    public async Task<bool> RemoveData(string key)
    {
        bool _isKeyExist = await _db.KeyExistsAsync(key);
        if (_isKeyExist == true)
        {
            return await _db.KeyDeleteAsync(key);
        }
        return false;
    }

    public async Task<bool> SetCacheData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        var isSet = await _db.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
        return isSet;
    }
}