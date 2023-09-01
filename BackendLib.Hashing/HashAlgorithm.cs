using System.Security.Cryptography;

namespace BackendLib.Hashing;

/// <summary>
/// Value type for hashing algorithm definitions.
/// </summary>
public sealed class HashAlgorithm
{
    #region Algorithms

    public static readonly HashAlgorithm SHA256 = new(HashAlgorithmName.SHA256);

    public static readonly HashAlgorithm SHA384 = new(HashAlgorithmName.SHA384);

    public static readonly HashAlgorithm SHA512 = new(HashAlgorithmName.SHA512);

    #endregion Algorithms

    public HashAlgorithmName AlgorithmName { get; }

    private HashAlgorithm(HashAlgorithmName algorithmName)
    {
        AlgorithmName = algorithmName;
    }

    private static readonly Dictionary<string, HashAlgorithmName> _supportedAlgorithms = new()
    {
        { "SHA256", HashAlgorithmName.SHA256 },
        { "SHA384", HashAlgorithmName.SHA384 },
        { "SHA512", HashAlgorithmName.SHA512 },
    };

    public static HashAlgorithm Parse(string algorithmName)
    {
        if (string.IsNullOrWhiteSpace(algorithmName) || !_supportedAlgorithms.ContainsKey(algorithmName))
        {
            throw new ArgumentException($"Invalid {nameof(algorithmName)}");
        }

        return new HashAlgorithm(_supportedAlgorithms[algorithmName]);
    }
}