namespace BackendLib;

/// <summary>
/// User friendly error message type
/// </summary>
public struct Error
{
    private readonly string? _key;
    private readonly string? _message;

    public string Key => _key ?? "Unexpected.Server.Error";

    public string Message => _message ?? "Unexpected Server Error";

    public Error(string key, string message)
    {
        _key = key;
        _message = message;
    }
}