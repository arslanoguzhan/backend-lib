namespace BackendLib.Tokens;

/// <summary>
/// Configuration of Token Service.
/// </summary>
public class TokenSettings
{
    public virtual string SignatureSecret { get; }

    public TokenSettings(string signatureSecret)
    {
        SignatureSecret = signatureSecret;
    }
}