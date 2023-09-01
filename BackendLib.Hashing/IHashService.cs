namespace BackendLib.Hashing;

/// <summary>
/// Mockable hashing service
/// </summary>
public interface IHashService
{
    string GenerateHash(string password);

    void ValidateHash(string hash, string password);

    string GenerateSignature(string data, string secret);

    void ValidateSignature(string signature, string data, string secret);
}