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

    public T GetCacheData<T>(string key)
    {
        var value = _db.StringGet(key);
        if (!string.IsNullOrEmpty(value))
        {
            return JsonSerializer.Deserialize<T>(value);
        }
        return default;
    }

    public bool RemoveData(string key)
    {
        bool _isKeyExist = _db.KeyExists(key);
        if (_isKeyExist == true)
        {
            return _db.KeyDelete(key);
        }
        return false;
    }

    public bool SetCacheData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        var isSet = _db.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
        return isSet;
    }
}