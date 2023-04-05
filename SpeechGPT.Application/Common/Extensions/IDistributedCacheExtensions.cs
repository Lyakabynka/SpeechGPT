using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

#pragma warning disable
namespace SpeechGPT.Application.Common.Extensions
{
    public static class IDistributedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
            string recordId,
            T data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null,
            CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),
                SlidingExpiration = unusedExpireTime
            };

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options, cancellationToken);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache,
            string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            
            if (jsonData is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
