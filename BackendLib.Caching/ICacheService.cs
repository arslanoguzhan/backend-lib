namespace BackendLib.Caching;

/// <summary>
/// Wrapper definition for caching services, designed for dependency inversion.
/// </summary>
public interface ICacheService
{
    Task<T> GetAsync<T>(string key);

    Task<List<string>> GetGroupsAsync();

    Task SetAsync<T>(string key, T value, IExpirationPolicy expirationPolicy);

    Task SetAsync<T>(string key, T value, IExpirationPolicy expirationPolicy, string group);

    Task RemoveAsync(string key);

    Task RemoveGroupAsync(string group);

    Task ClearAsync();
}