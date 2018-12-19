using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Canary.Management.Api
{
    public class CachingService : ICachingService
    {
        private readonly IDistributedCache cache;
        private readonly HashSet<string> keys;

        public CachingService(IDistributedCache cache)
        {
            this.cache = cache;
            this.keys = new HashSet<string>();
        }

        public async Task<T> TryGetFromCache<T>(Func<Task<T>> toRun, string cacheKey)
        {
            string json = await this.cache.GetStringAsync(cacheKey);
            if (json == null)
            {
                var result = await toRun();
                json = JsonConvert.SerializeObject(result);
                await this.cache.SetStringAsync(cacheKey, json);
                this.keys.Add(cacheKey);
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task Clear()
        {
            foreach (var key in this.keys)
            {
                await this.cache.RemoveAsync(key);
            }
            this.keys.Clear();
        }

        public async Task Invalidate(string cacheKey)
        {
            await this.cache.RemoveAsync(cacheKey);
        }
    }
}
