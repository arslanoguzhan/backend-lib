namespace BackendLib.Tokens;

/// <summary>
/// Generates, decodes and validates auth tokens.
/// </summary>
public interface ITokenService
{
    string GenerateToken<TPayload>(TPayload payload, ITokenPolicy tokenPolicy) where TPayload : class;

    Token<TPayload> DecodeToken<TPayload>(string tokenString) where TPayload : class;
}