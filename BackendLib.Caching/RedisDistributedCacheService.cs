using System.Text;
using BackendLib.Json;
using StackExchange.Redis;

namespace BackendLib.Caching;

/// <summary>
/// Mockable wrapper for Stackexchange.Redis to implement ICacheService.
/// </summary>
public class RedisDistributedCacheService : ICacheService
{
    private readonly IJsonService _jsonService;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisDistributedCacheService(IJsonService jsonService, IConnectionMultiplexer connectionMultiplexer)
    {
        _jsonService = jsonService;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        try
        {
            var database = _connectionMultiplexer.GetDatabase();

            byte[]? wrappedBytes = await database.StringGetAsync(key).ConfigureAwait(false);

            var wrappedJson = Encoding.UTF8.GetString(wrappedBytes ?? Array.Empty<byte>());
            var wrapped = _jsonService.Deserialize<CacheWrapper<T>>(wrappedJson);
            return wrapped.Value;
        }
        catch
        {
            await RemoveAsync(key).ConfigureAwait(false);
            throw new Exception($"cache key failed: {key}");
        }
    }

    public async Task<List<string>> GetGroupsAsync()
    {
        try
        {
            var database = _connectionMultiplexer.GetDatabase();

            var groups = await database.SetMembersAsync("groups").ConfigureAwait(false);

            return groups.Select(g => ((string?)g) ?? "").ToList();
        }
        catch
        {
            throw new Exception("cache groups failed");
        }
    }

    public async Task SetAsync<T>(string key, T value, IExpirationPolicy expirationPolicy)
    {
        var database = _connectionMultiplexer.GetDatabase();

        var wrapped = new CacheWrapper<T>(value);
        var wrappedJson = _jsonService.Serialize(wrapped);
        var wrappedBytes = Encoding.UTF8.GetBytes(wrappedJson);

        await database.StringSetAsync(key, wrappedBytes, expirationPolicy.ExpiresIn).ConfigureAwait(false);
    }

    public async Task SetAsync<T>(string key, T value, IExpirationPolicy expirationPolicy, string group)
    {
        await SetAsync(key, value, expirationPolicy).ConfigureAwait(false);

        var database = _connectionMultiplexer.GetDatabase();

        await database.SetAddAsync(group, key).ConfigureAwait(false);
        await database.SetAddAsync("groups", group).ConfigureAwait(false);
    }

    public async Task RemoveAsync(string key)
    {
        var database = _connectionMultiplexer.GetDatabase();

        await database.KeyDeleteAsync(key).ConfigureAwait(false);
    }

    public async Task RemoveGroupAsync(string group)
    {
        var database = _connectionMultiplexer.GetDatabase();

        string?[] members = (await database.SetMembersAsync(group).ConfigureAwait(false)).ToStringArray() ?? Array.Empty<string?>();

        foreach (var key in members)
        {
            await RemoveAsync(key ?? "").ConfigureAwait(false);
        }

        await database.SetRemoveAsync("groups", group).ConfigureAwait(false);
    }

    public async Task ClearAsync()
    {
        var endpoints = _connectionMultiplexer.GetEndPoints(true);

        await Parallel.ForEachAsync(endpoints, async (endpoint, _) =>
        {
            var server = _connectionMultiplexer.GetServer(endpoint);

            await server.FlushAllDatabasesAsync();
        });
    }
}