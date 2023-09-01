namespace BackendLib.Caching;

/// <summary>
/// Definition of cache lifespan
/// </summary>
public interface IExpirationPolicy
{
    TimeSpan? ExpiresIn { get; }
}