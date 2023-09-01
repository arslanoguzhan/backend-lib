namespace BackendLib.Encryption;

/// <summary>
/// Mockable encryption service.
/// </summary>
public interface IEncryptionService
{
    string Encrypt(string plainText, string secret);

    string Decrypt(string cipherText, string secret);
}