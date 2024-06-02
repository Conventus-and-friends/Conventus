using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Conventus.Server.Extensions;

// FIXME: do not use try catch for this
public static class IDistributedCacheExtensions
{
    public static T GetOrCreate<T>(this IDistributedCache cache, string key, Func<T> factory, DistributedCacheEntryOptions? options = null)
    {
        var cacheEntry = cache.GetString(key);
        if (cacheEntry is null)
        {
            var result = factory();
            try
            {
                cacheEntry = JsonSerializer.Serialize(result);
            }
            catch (NotSupportedException)
            {
                return result;
            }
            cache.SetString(key, cacheEntry, options ?? new());
            return result;
        }

        try
        {
            return JsonSerializer.Deserialize<T>(cacheEntry)!;
        }
        catch (NotSupportedException)
        {
            return factory();
        }
    }

    public static async Task<T> GetOrCreateAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> factory, DistributedCacheEntryOptions? options = null)
    {
        var cacheEntry = await cache.GetStringAsync(key);
        if (cacheEntry is null)
        {
            var result = await factory();
            try
            {
                cacheEntry = JsonSerializer.Serialize(result);
            }
            catch (NotSupportedException)
            {
                return result;
            }
            await cache.SetStringAsync(key, cacheEntry, options ?? new());
            return result;
        }

        try
        {
            return JsonSerializer.Deserialize<T>(cacheEntry)!;
        }
        catch (NotSupportedException)
        {
            return await factory();
        }
    }
}
