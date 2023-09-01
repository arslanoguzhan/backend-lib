using BackendLib;
using BackendLib.Base64;
using BackendLib.Caching;
using BackendLib.Cookies;
using BackendLib.Encryption;
using BackendLib.Hashing;
using BackendLib.Json;
using BackendLib.Time;
using BackendLib.Tokens;
using BackendLib.UsageExamples.Cookies;
using BackendLib.UsageExamples.Mocks;
using BackendLib.UsageExamples.Policies;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BackendLib.UsageExamples;

internal class Program
{
    static async Task Main(string[] args)
    {
        Env env = Env.Local;

        ServiceCollection services = new ServiceCollection();

        var tokenSettings = new TokenSettings("secret");

        // register basic services
        services.AddSingleton<Env>(env);
        services.AddTransient<ITimeProvider, TimeProvider>();
        services.AddTransient<IBase64Service, Base64Service>();
        services.AddTransient<IJsonService, NewtonsoftJsonService>();
        services.AddTransient<IEncryptionService, EncryptionService>();
        services.AddTransient<IHashService, HashService>();

        // register cookies
        services.AddHttpContextAccessor();
        services.AddSingleton<AccessTokenCookie>();
        services.AddSingleton<RefreshTokenCookie>();
        //services.AddTransient(typeof(ICookieService<>), typeof(CookieService<>));
        services.AddTransient(typeof(ICookieService<>), typeof(MockCookieService<>)); // console apps do not have http context

        // register auth tokens
        services.AddSingleton<AccessTokenPolicy>();
        services.AddSingleton<RefreshTokenPolicy>();
        services.AddSingleton<TokenSettings>(tokenSettings);
        services.AddTransient<ITokenService, TokenService>();

        // register caching (redis)
        //services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("redis-uri"));
        //services.AddTransient<ICacheService, RedisDistributedCacheService>();

        // register caching (microsoft local cache)
        services.AddMemoryCache();
        services.AddTransient<ICacheService, MicrosoftMemoryLocalCacheService>();

        // register http functions
        services.AddTransient<LoginFunction>();

        // build
        ServiceProvider serviceProvider = services.BuildServiceProvider();

        //
        // login example (faking an http request)
        //
        using (var scope =  serviceProvider.CreateScope())
        {
            // assume we got an http-request to /login
            var loginFunction = scope.ServiceProvider.GetService<LoginFunction>();

            Console.WriteLine("Enter the password (123456) for -admin-:");
            var pass = Console.ReadLine() ?? "";

            // and assume this is the request data
            var requestDataExample = new LoginFunction.LoginRequest()
            {
                Username = "admin",
                Password = pass
            };

            var response = await loginFunction.ExecuteAsync(requestDataExample);

            // at this point, assume writing "response" to the http-response body.

            if (response.Errors.Any())
            {
                Console.WriteLine("login failed: " + response.Errors.First().Key + " - " + response.Errors.First().Message);
            }
            else
            {
                Console.WriteLine("login successful, redirecting to: " + response.Data.RedirectUri);
            }
        }

        Console.WriteLine("");
        Console.WriteLine("Press Enter to Exit...");

        Console.ReadLine();
    }
}