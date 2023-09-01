using Microsoft.AspNetCore.Http;

namespace BackendLib.Cookies;

/// <summary>
/// Default implementation of ICookieService. Lets developers access cookies easily.
/// </summary>
/// <typeparam name="TCookie"></typeparam>
public class CookieService<TCookie> : ICookieService<TCookie>
    where TCookie : ICookie
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TCookie _cookie;
    private readonly Env _env;

    public CookieService(IHttpContextAccessor httpContextAccessor, TCookie cookie, Env env)
    {
        _httpContextAccessor = httpContextAccessor;
        _cookie = cookie;
        _env = env;
    }

    public string Read()
    {
        string? value;
        try
        {
            var cookieKey = $"{_env.CookieKeyPrefix}-{_cookie.Key}";
            _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(cookieKey, out value);
            value ??= "";
        }
        catch
        {
            value = "";
        }

        return value;
    }

    public void Write(string value)
    {
        var cookieKey = $"{_env.CookieKeyPrefix}-{_cookie.Key}";
        _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieKey, value, _cookie.Options);
    }

    public void Delete()
    {
        var cookieKey = $"{_env.CookieKeyPrefix}-{_cookie.Key}";
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieKey, _cookie.Options);
    }
}