namespace BackendLib.Hashing;

/// <summary>
/// Definition for hashing configurations.
/// </summary>
public interface IHashConfig
{
    HashAlgorithm Algorithm { get; }

    ByteLength KeyLength { get; }

    ByteLength SaltLength { get; }

    int Iterations { get; }
}