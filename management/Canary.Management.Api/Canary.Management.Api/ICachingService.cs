using System;
using System.Threading.Tasks;

namespace Canary.Management.Api
{
    public interface ICachingService
    {
        Task Invalidate(string cacheKey);
        Task<T> TryGetFromCache<T>(Func<Task<T>> toRun, string cacheKey);
        Task Clear();
    }
}