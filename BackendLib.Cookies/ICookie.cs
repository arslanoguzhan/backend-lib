using Microsoft.AspNetCore.Http;

namespace BackendLib.Cookies;

/// <summary>
/// Definition for cookie values
/// </summary>
public interface ICookie
{
    string Key { get; }

    CookieOptions Options { get; }
}