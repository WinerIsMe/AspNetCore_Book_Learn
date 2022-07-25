using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swift.BBS.Common.MemoryCache
{
    /// <summary>
    /// 实例化缓存接口
    /// </summary>
    public class MemoryCachingProvider : ICachingProvider
    {
        private readonly IMemoryCache cache;

        public MemoryCachingProvider(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public object Get(string cacheKey)
        {
            return cache.Get(cacheKey);
        }

        public void Set(string cacheKey, object cacheValue)
        {
            cache.Set(cacheKey, cacheValue, TimeSpan.FromSeconds(7200));
        }
    }
}
