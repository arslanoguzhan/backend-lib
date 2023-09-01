using BackendLib.Time;

namespace BackendLib.Tokens;

/// <summary>
/// Data type for Auth token.
/// </summary>
/// <typeparam name="TPayload">Type of token payload</typeparam>
public class Token<TPayload> where TPayload : class
{
    public Guid TokenGuid { get; }

    public long ExpiresAt { get; }

    public TPayload? Payload { get; }

    public TokenStatus GetStatus() => TokenGuid == Guid.Empty ? TokenStatus.Empty
        : ExpiresAt < TimeProvider.UtcNow.ToUnixTimeSeconds() ? TokenStatus.Expired
        : TokenStatus.Valid;

    public static readonly Token<TPayload> Empty = new(Guid.Empty, 0, null);

    public Token(Guid tokenGuid, long expiresAt, TPayload? payload)
    {
        TokenGuid = tokenGuid;
        ExpiresAt = expiresAt;
        Payload = payload;
    }
}