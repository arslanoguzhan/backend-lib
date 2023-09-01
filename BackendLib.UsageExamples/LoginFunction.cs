using BackendLib.Cookies;
using BackendLib.Hashing;
using BackendLib.Http;
using BackendLib.Tokens;
using BackendLib.UsageExamples.Cookies;
using BackendLib.UsageExamples.Models;
using BackendLib.UsageExamples.Policies;
using static BackendLib.UsageExamples.LoginFunction;

namespace BackendLib.UsageExamples
{
    internal class LoginFunction : IHttpFunction<LoginRequest, LoginResponse>
    {
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;
        private readonly ICookieService<AccessTokenCookie> _accessTokenCookieService;
        private readonly ICookieService<RefreshTokenCookie> _refreshTokenCookieService;
        private readonly AccessTokenPolicy _accessTokenPolicy;
        private readonly RefreshTokenPolicy _refreshTokenPolicy;

        public LoginFunction(
            IHashService hashService,
            ITokenService tokenService,
            ICookieService<AccessTokenCookie> accessTokenCookieService,
            ICookieService<RefreshTokenCookie> refreshTokenCookieService,
            AccessTokenPolicy accessTokenPolicy,
            RefreshTokenPolicy refreshTokenPolicy
            )
        {
            _hashService = hashService;
            _tokenService = tokenService;
            _accessTokenCookieService = accessTokenCookieService;
            _refreshTokenCookieService = refreshTokenCookieService;
            _accessTokenPolicy = accessTokenPolicy;
            _refreshTokenPolicy = refreshTokenPolicy;
        }

        // request data model of Login
        public class LoginRequest
        {
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
        }

        // response data model of Login
        public class LoginResponse
        {
            public string RedirectUri { get; set; } = "";
        }

        public async Task<ApiResponse<LoginResponse>> ExecuteAsync(LoginRequest request)
        {
            var userFromDB = await GetUserFromDatabase(request);

            try
            {
                // validate password and hash
                _hashService.ValidateHash(userFromDB.PasswordHash, request.Password);
            }
            catch (Exception ex)
            {
                // invalid username or password
                return new ApiResponse<LoginResponse>(new Error[] { new Error("Login failed", "invalid username or password") });
            }

            // correct username and password

            // create tokens
            var tokenPayload = new TokenPayload() { Username = request.Username };
            var accessTokenString = _tokenService.GenerateToken(tokenPayload, _accessTokenPolicy);
            var refreshTokenString = _tokenService.GenerateToken(tokenPayload, _refreshTokenPolicy);

            // write tokens to cookies
            _accessTokenCookieService.Write(accessTokenString);
            _refreshTokenCookieService.Write(refreshTokenString);

            return new ApiResponse<LoginResponse>(new LoginResponse() { RedirectUri = "/" });
        }

        private Task<User> GetUserFromDatabase(LoginRequest request)
        {
            // There is no db, here we are faking a db call.

            var hash = _hashService.GenerateHash("123456");

            return Task.FromResult(new User()
            {
                Username = request.Username,
                PasswordHash = hash
            });
        }
    }
}
