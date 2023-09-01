namespace BackendLib.Caching;

/// <summary>
/// The type to determine the lifespan of a cache.
/// </summary>
public class ExpirationPolicy : IExpirationPolicy
{
    public TimeSpan? ExpiresIn { get; }

    public ExpirationPolicy(TimeSpan? expiresIn)
    {
        ExpiresIn = expiresIn;
    }
}