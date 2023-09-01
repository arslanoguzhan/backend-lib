namespace BackendLib.Hashing;

/// <summary>
/// Value type for hashing configurations.
/// </summary>
public sealed class HashConfig : IHashConfig
{
    public static HashConfig Default => SHA256;

    public static readonly HashConfig SHA256 = new(
        HashAlgorithm.SHA256,
        ByteLength.Bits_256,
        ByteLength.Bits_128,
        50000);

    public static readonly HashConfig SHA384 = new(
        HashAlgorithm.SHA384,
        ByteLength.Bits_384,
        ByteLength.Bits_256,
        50000);

    public static readonly HashConfig SHA512 = new(
        HashAlgorithm.SHA512,
        ByteLength.Bits_512,
        ByteLength.Bits_256,
        50000);

    public HashAlgorithm Algorithm { get; }

    public ByteLength KeyLength { get; }

    public ByteLength SaltLength { get; }

    public int Iterations { get; }

    private HashConfig(HashAlgorithm algorithm, ByteLength keyLength, ByteLength saltLength, int iterations)
    {
        Algorithm = algorithm;
        KeyLength = keyLength;
        SaltLength = saltLength;
        Iterations = iterations;
    }
}