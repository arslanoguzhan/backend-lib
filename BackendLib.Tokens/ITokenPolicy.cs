namespace BackendLib.Tokens;

/// <summary>
/// Expiration policy of auth tokens, lets you assign different policies to different tokens e.g. access-token and refresh-token
/// </summary>
public interface ITokenPolicy
{
    TimeSpan LifeSpan { get; }
}