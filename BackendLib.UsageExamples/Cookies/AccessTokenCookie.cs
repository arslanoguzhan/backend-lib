using BackendLib.Cookies;
using BackendLib.Time;
using Microsoft.AspNetCore.Http;

namespace BackendLib.UsageExamples.Cookies;

public class AccessTokenCookie : ICookie
{
    private readonly Env _env;
    private readonly ITimeProvider _timeProvider;

    public AccessTokenCookie(Env env, ITimeProvider timeProvider)
    {
        _env = env;
        _timeProvider = timeProvider;
    }

    public string Key => "access-token";

    public CookieOptions Options => new()
    {
        Expires = _timeProvider.GetUtcNow().AddYears(10), // expiration of browser cookie, not the token itself
        HttpOnly = true,
        IsEssential = true,
        SameSite = SameSiteMode.Strict,
        Secure = true,
        Domain = $".{_env.Domain}",
        Path = "/",
    };
}