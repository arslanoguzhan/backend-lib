using System.Text;

namespace BackendLib.Base64;

/// <summary>
/// Mockable wrapper for base64 encoding, with default implementation
/// </summary>
public class Base64Service : IBase64Service
{
    public string Encode(string value)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
    }

    public string Decode(string value)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(value));
    }
}