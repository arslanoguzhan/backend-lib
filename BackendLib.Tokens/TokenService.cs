using BackendLib.Base64;
using BackendLib.Time;
using BackendLib.Hashing;
using BackendLib.Json;

namespace BackendLib.Tokens;

/// <summary>
/// Default implementation of ITokenService.
/// </summary>
public class TokenService : ITokenService
{
    private readonly TokenSettings _tokenSettings;
    private readonly IHashService _hashService;
    private readonly IJsonService _jsonService;
    private readonly IBase64Service _base64Service;
    private readonly ITimeProvider _timeProvider;

    public TokenService(
        TokenSettings tokenSettings,
        IHashService hashService,
        IJsonService jsonService,
        IBase64Service base64Service,
        ITimeProvider timeProvider)
    {
        _tokenSettings = tokenSettings;
        _hashService = hashService;
        _jsonService = jsonService;
        _base64Service = base64Service;
        _timeProvider = timeProvider;
    }

    public string GenerateToken<TPayload>(TPayload payload, ITokenPolicy tokenPolicy) where TPayload : class
    {
        long expiresAt = _timeProvider.GetUtcNow().Add(tokenPolicy.LifeSpan).ToUnixTimeSeconds();
        var payloadObject = new Token<TPayload>(Guid.NewGuid(), expiresAt, payload);

        var secret = _tokenSettings.SignatureSecret;

        var payloadJson = _jsonService.Serialize(payloadObject);
        var payloadJsonB64 = _base64Service.Encode(payloadJson);

        var payloadJsonHash = _hashService.GenerateSignature(payloadJson, secret);

        return $"{payloadJsonB64}.{payloadJsonHash}";
    }

    public Token<TPayload> DecodeToken<TPayload>(string tokenString) where TPayload : class
    {
        try
        {
            if (string.IsNullOrWhiteSpace(tokenString))
            {
                return Token<TPayload>.Empty;
            }

            var parts = tokenString.Split('.');
            var payloadJsonB64 = parts[0];
            var payloadJsonHash = parts[1];

            var payloadJson = _base64Service.Decode(payloadJsonB64);

            var secret = _tokenSettings.SignatureSecret;

            _hashService.ValidateSignature(payloadJsonHash, payloadJson, secret);

            return _jsonService.Deserialize<Token<TPayload>>(payloadJson);
        }
        catch
        {
            return Token<TPayload>.Empty;
        }
    }
}