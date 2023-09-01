namespace BackendLib.Base64;

/// <summary>
/// Mockable wrapper for base64 encoding
/// </summary>
public interface IBase64Service
{
    string Encode(string value);

    string Decode(string value);
}