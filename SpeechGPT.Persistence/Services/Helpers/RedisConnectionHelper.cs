using Microsoft.Extensions.Options;
using SpeechGPT.Application.Options;
using StackExchange.Redis;

namespace SpeechGPT.Persistence.Services.Helpers;

public class RedisConnectionHelper
{
    private readonly RedisOptions _options;
    
    private Lazy<ConnectionMultiplexer> lazyConnection;
    public ConnectionMultiplexer Connection => 
        lazyConnection.Value;

    public RedisConnectionHelper(IOptions<RedisOptions> options)
    {
        _options = options.Value;
        
        lazyConnection = new Lazy<ConnectionMultiplexer>(() => 
            ConnectionMultiplexer.Connect(_options.Url));
    }
}