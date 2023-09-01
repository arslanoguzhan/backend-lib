namespace BackendLib.Caching;

/// <summary>
/// A wrapper type to make it possible to Json-serialize scalar values
/// </summary>
/// <typeparam name="T">Type of the cache data</typeparam>
internal class CacheWrapper<T>
{
    public T Value { get; }

    public CacheWrapper(T value)
    {
        Value = value;
    }
}