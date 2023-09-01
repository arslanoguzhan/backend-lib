using System.Security.Cryptography;

namespace BackendLib.Hashing;

/// <summary>
/// Default implementation of IHashService
/// </summary>
public class HashService : IHashService
{
    public string GenerateHash(string password)
    {
        var config = HashConfig.Default;

        var saltPhrase = DataPhrase.Generate(config.SaltLength);
        var passwordPhrase = DataPhrase.ParseText(password);

        byte[] hashKeyBytes = ComputeHash(
            password: passwordPhrase,
            salt: saltPhrase,
            algorithm: config.Algorithm,
            keyLength: config.KeyLength,
            iterations: config.Iterations);

        var keyPhrase = DataPhrase.ParseBytes(hashKeyBytes);

        return $"{config.Algorithm.AlgorithmName.Name}.{config.Iterations}.{saltPhrase.Base64}.{keyPhrase.Base64}";
    }

    private static byte[] ComputeHash(DataPhrase password, DataPhrase salt, HashAlgorithm algorithm, ByteLength keyLength, int iterations)
    {
        using var computation = new Rfc2898DeriveBytes(
            password.Bytes,
            salt.Bytes,
            iterations,
            algorithm.AlgorithmName);

        return computation.GetBytes((int)keyLength);
    }

    public string GenerateSignature(string data, string secret)
    {
        var signatureKeyBytes = ComputeSignature(data, secret);
        var dataHashPhrase = DataPhrase.ParseBytes(signatureKeyBytes);

        return dataHashPhrase.Base64;
    }

    private static byte[] ComputeSignature(string data, string secret)
    {
        byte[] dataBytes = DataPhrase.ParseText(data).Bytes;
        byte[] secretBytes = DataPhrase.ParseText(secret).Bytes;

        using var signatureGenerator = new HMACSHA256(secretBytes);
        byte[] dataHashBytes = signatureGenerator.ComputeHash(dataBytes);

        return dataHashBytes;
    }

    public void ValidateHash(string hash, string password)
    {
        try
        {
            var givenHash = new HashValue(hash);
            var computedHashBytes = ComputeHash(DataPhrase.ParseText(password), givenHash.Salt, givenHash.Algorithm, (ByteLength)givenHash.Key.Bytes.Length, givenHash.Iterations);

            if (!givenHash.Key.Bytes.SequenceEqual(computedHashBytes))
            {
                throw new Exception("invalid hash format");
            }
        }
        catch (Exception e)
        {
            throw new Exception("hash validation failed", e);
        }
    }

    public void ValidateSignature(string signature, string data, string secret)
    {
        try
        {
            var givenSignature = DataPhrase.ParseBase64(signature);
            var computedSignatureBytes = ComputeSignature(data, secret);

            if (!givenSignature.Bytes.SequenceEqual(computedSignatureBytes))
            {
                throw new Exception("invalid signature format");
            }
        }
        catch (Exception e)
        {
            throw new Exception("signature validation failed", e);
        }
    }
}