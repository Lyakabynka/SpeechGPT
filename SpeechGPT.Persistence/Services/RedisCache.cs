using System.Text.Json;
using SpeechGPT.Application.CQRS.Queries.ViewModels;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Persistence.Services.Helpers;
using StackExchange.Redis;

namespace SpeechGPT.Persistence.Services;

public class RedisCache : IRedisCache
{
    private IDatabase? _db;
    private readonly RedisConnectionHelper _connectionHelper;
    
    public RedisCache(RedisConnectionHelper connectionHelper)
    {
        _connectionHelper = connectionHelper;
        ConfigureRedis();
    }
    
    private void ConfigureRedis()
    {
        _db = _connectionHelper.Connection?.GetDatabase();
    }

    private bool IsConnected()
    {
        return _db is not null && _connectionHelper.Connection.IsConnected;
    }

    public async Task<T?> GetCacheData<T>(string key)
    {
        if (!IsConnected())
            return default(T);

        var value = await _db.StringGetAsync(key);
        if (!string.IsNullOrEmpty(value))
        {
            return JsonSerializer.Deserialize<T>(value);
        }
        return default(T);
    }

    public async Task<bool> RemoveData(string key)
    {
        if (!IsConnected())
            return false;
        
        bool isKeyExist = await _db.KeyExistsAsync(key);
        if (isKeyExist == true)
            return await _db.KeyDeleteAsync(key);
        
        return false;
    }

    public async Task<bool> SetCacheData<T>(string key, T value, DateTime expirationTime = default)
    {
        if (!IsConnected())
            return false;
        
        TimeSpan expires;
        if (expirationTime == default)
            expires = TimeSpan.FromHours(1);
        else
            expires = expirationTime.Subtract(DateTime.Now);
        
        var isSet = await _db.StringSetAsync(key, JsonSerializer.Serialize(value), expires);
        return isSet;
    }
    
    public async Task<long> ListAppend<T>(string key, T value, DateTime expirationTime = default)
    {
        if (!IsConnected() || !(await _db.KeyExistsAsync(key)))
            return 0;
        
        if (expirationTime == default)
            expirationTime = DateTime.Now.AddHours(1);
        
        long countElements = await _db.ListRightPushAsync(key, JsonSerializer.Serialize(value));
        _db.KeyExpire(key, expirationTime);
        return countElements;
    }

    public async Task<List<T>?> GetList<T>(string key)
    {
        if (!IsConnected())
            return null;
        
        var elements = await _db.ListRangeAsync(key,0,-1);
        if (elements.Length == 0)
            return null;

        return elements.Select(element => 
                JsonSerializer.Deserialize<T>(element)!)
            .ToList();
    }
}
