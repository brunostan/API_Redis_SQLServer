using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BookStore.Services
{
    public class CacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetFromCacheAsync<T>(string key)
        {
            string? cachedData = await _cache.GetStringAsync(key);
            return cachedData != null ? JsonSerializer.Deserialize<T>(cachedData) : default;
        }

        public async Task SetCacheAsync<T>(string key, T data)
        {
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(1));
            string serializedData = JsonSerializer.Serialize(data);
            await _cache.SetStringAsync(key, serializedData, options);
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}