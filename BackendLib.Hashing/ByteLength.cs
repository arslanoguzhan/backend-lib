namespace BackendLib.Hashing;

public enum ByteLength
{
    None = 0,

    Bytes_16 = 16,
    Bits_128 = Bytes_16,

    Bytes_32 = 32,
    Bits_256 = Bytes_32,

    Bytes_48 = 48,
    Bits_384 = Bytes_48,

    Bytes_64 = 64,
    Bits_512 = Bytes_64,
}