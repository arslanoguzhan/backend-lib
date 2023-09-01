using System.Security.Cryptography;

namespace BackendLib.Encryption;

/// <summary>
/// Default implementation of IEncryptionService.
/// </summary>
public class EncryptionService : IEncryptionService
{
    private const int SaltSize = 32;

    public string Encrypt(string plainText, string key)
    {
        if (string.IsNullOrEmpty(plainText))
        {
            throw new ArgumentNullException(nameof(plainText));
        }

        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        // Derive a new Salt and IV from the Key
        using var keyDerivationFunction = new Rfc2898DeriveBytes(key, SaltSize);
        var saltBytes = keyDerivationFunction.Salt;
        var keyBytes = keyDerivationFunction.GetBytes(32);
        var ivBytes = keyDerivationFunction.GetBytes(16);

        // Create an encryptor to perform the stream transform.
        // Create the streams used for encryption.
        using var aes = Aes.Create();
        using var encryptor = aes.CreateEncryptor(keyBytes, ivBytes);
        using var memoryStream = new MemoryStream();
        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        using (var streamWriter = new StreamWriter(cryptoStream))
        {
            // Send the data through the StreamWriter, through the CryptoStream, to the underlying MemoryStream
            streamWriter.Write(plainText);
        }

        // Return the encrypted bytes from the memory stream, in Base64 form so we can send it right to a database (if we want).
        var cipherTextBytes = memoryStream.ToArray();
        Array.Resize(ref saltBytes, saltBytes.Length + cipherTextBytes.Length);
        Array.Copy(cipherTextBytes, 0, saltBytes, SaltSize, cipherTextBytes.Length);

        return Convert.ToBase64String(saltBytes);
    }

    public string Decrypt(string cipherText, string key)
    {
        if (string.IsNullOrEmpty(cipherText))
        {
            throw new ArgumentNullException(nameof(cipherText));
        }

        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        // Extract the salt from our cipherText
        var allTheBytes = Convert.FromBase64String(cipherText);
        var saltBytes = allTheBytes.Take(SaltSize).ToArray();
        var cipherTextBytes = allTheBytes.Skip(SaltSize).Take(allTheBytes.Length - SaltSize).ToArray();

        using var keyDerivationFunction = new Rfc2898DeriveBytes(key, saltBytes);
        // Derive the previous IV from the Key and Salt
        var keyBytes = keyDerivationFunction.GetBytes(32);
        var ivBytes = keyDerivationFunction.GetBytes(16);

        // Create a decryptor to perform the stream transform.
        // Create the streams used for decryption.
        // The default Cipher Mode is CBC and the Padding is PKCS7 which are both good
        using var aes = Aes.Create();
        using var decryptor = aes.CreateDecryptor(keyBytes, ivBytes);
        using var memoryStream = new MemoryStream(cipherTextBytes);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        // Return the decrypted bytes from the decrypting stream.
        return streamReader.ReadToEnd();
    }
}