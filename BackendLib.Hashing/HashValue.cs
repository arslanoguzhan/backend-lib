namespace BackendLib.Hashing;

/// <summary>
/// Value type for hash strings.
/// </summary>
public class HashValue
{
    public HashAlgorithm Algorithm { get; }

    public DataPhrase Salt { get; }

    public DataPhrase Key { get; }

    public int Iterations { get; }

    public HashValue(string hash)
    {
        var parts = hash.Split('.');

        if (parts.Length != 4)
        {
            throw new FormatException("Unexpected hash format. Should be formatted as {algorithm}.{iterations}.{salt}.{hash}");
        }

        var algorithmName = parts[0];
        var iterations = Convert.ToInt32(parts[1]);
        var salt = DataPhrase.ParseBase64(parts[2]);
        var key = DataPhrase.ParseBase64(parts[3]);

        Algorithm = HashAlgorithm.Parse(algorithmName);
        Salt = salt;
        Key = key;
        Iterations = iterations;
    }
}