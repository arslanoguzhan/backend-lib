using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;

namespace BackendLib.Caching;

public class MicrosoftMemoryLocalCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    private readonly ConcurrentDictionary<string, HashSet<string>> _groups = new();

    public MicrosoftMemoryLocalCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Task<T> GetAsync<T>(string key)
    {
        var isHit = _memoryCache.TryGetValue(key, out var result);

        if (!isHit || result is not CacheWrapper<T> wrapped)
        {
            throw new Exception(key);
        }

        return Task.FromResult(wrapped.Value);
    }

    public async Task<List<string>> GetGroupsAsync()
    {
        List<string> groups;

        lock (_groups)
        {
            groups = _groups.Keys.ToList();
        }

        return await Task.FromResult(groups);
    }

    public Task SetAsync<T>(string key, T value, IExpirationPolicy expirationPolicy)
    {
        var entry = _memoryCache.CreateEntry(key);
        entry.Value = new CacheWrapper<T>(value);
        entry.SlidingExpiration = expirationPolicy.ExpiresIn;

        entry.Dispose();

        return Task.CompletedTask;
    }

    public Task SetAsync<T>(string key, T value, IExpirationPolicy expirationPolicy, string group)
    {
        SetAsync(key, value, expirationPolicy).ConfigureAwait(false);

        lock (_groups)
        {
            if (_groups.TryGetValue(group, out var groupKeys))
            {
                groupKeys.Add(key);
            }
            else
            {
                _groups.TryAdd(group, new HashSet<string> {key});
            }
        }

        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _memoryCache.Remove(key);

        return Task.CompletedTask;
    }

    public Task RemoveGroupAsync(string group)
    {
        lock (_groups)
        {
            if (!_groups.TryGetValue(group, out var groupKeys))
            {
                return Task.CompletedTask;
            }

            foreach (var key in groupKeys)
            {
                RemoveAsync(key);
            }

            _groups.TryRemove(group, out _);
        }

        return Task.CompletedTask;
    }

    public async Task ClearAsync()
    {
        var caches = await GetGroupsAsync();

        Parallel.ForEach(caches, cache => RemoveGroupAsync(cache));
    }
}