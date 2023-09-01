using System.Security.Cryptography;
using System.Text;

namespace BackendLib.Hashing;

/// <summary>
/// Prepares data to be ready for hashing.
/// </summary>
public sealed class DataPhrase
{
    public string Base64 { get; }

    public byte[] Bytes { get; }

    private DataPhrase(byte[] bytes, string base64)
    {
        Bytes = bytes;
        Base64 = base64;
    }

    public static DataPhrase Generate(ByteLength length)
    {
        var bytes = new byte[(int)length];

        RandomNumberGenerator.Fill(bytes);

        return ParseBytes(bytes);
    }

    public static DataPhrase ParseBytes(byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        return new DataPhrase(bytes, Convert.ToBase64String(bytes));
    }

    public static DataPhrase ParseText(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);

        return new DataPhrase(bytes, Convert.ToBase64String(bytes));
    }

    public static DataPhrase ParseBase64(string base64String)
    {
        var bytes = Convert.FromBase64String(base64String);

        return new DataPhrase(bytes, base64String);
    }
}