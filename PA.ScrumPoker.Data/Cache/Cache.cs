using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PA.ScrumPoker.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.ScrumPoker.Data.Cache
{
    public class Cache<T> : ICache<T>
          where T : IAmADto
    {
        private readonly IDistributedCache _cache;
        private readonly CacheOptions _options;

        public Cache(IDistributedCache cache, IOptions<CacheOptions> options)
        {
            _cache = cache;
            _options = options.Value;
        }

        public IEnumerable<T> GetCache(string cacheKey)
        {
            var cacheValue = _cache.GetString(cacheKey);

            if (cacheValue != null)
            {
                return JsonConvert.DeserializeObject<List<T>>(cacheValue);
            }

            return null;
        }

        public void SetCache(string cacheKey, IEnumerable<T> sourceEntity)
        {
            if (sourceEntity != null)
            {
                _cache.SetString(cacheKey, JsonConvert.SerializeObject(sourceEntity), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _options.Expiration
                });
            }
            else
            {
                RemoveCache(cacheKey);
            }
        }

        public void RemoveCache(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }
    }
}
