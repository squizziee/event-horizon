using EventHorizon.Application.UseCases.Interfaces;
using EventHorizon.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EventHorizon.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IRegisterUserUseCase _registerUserUseCase;
        private readonly ILoginUseCase _loginUseCase;
        private readonly IRefreshTokensUseCase _refreshTokensUseCase;
        private readonly CookieOptions _accessTokenCookieOptions;
        private readonly CookieOptions _refreshTokenCookieOptions;
        public AuthenticationController(
            IRegisterUserUseCase registerUserUseCase,
            ILoginUseCase loginUseCase,
            IRefreshTokensUseCase refreshTokensUseCase,
            IConfiguration configuration) {
            _registerUserUseCase = registerUserUseCase;
            _loginUseCase = loginUseCase;
            _refreshTokensUseCase = refreshTokensUseCase;

            _accessTokenCookieOptions = new()
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(
                    double.Parse(configuration["Jwt:AccessToken:ExpirationTimeInMinutes"]!)),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None
            };

            _refreshTokenCookieOptions = new()
            {
                Expires = DateTimeOffset.UtcNow.AddDays(
                    double.Parse(configuration["Jwt:RefreshToken:ExpirationTimeInDays"]!)),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None
            };
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(
            [FromForm] RegsiterUserRequest request, 
            CancellationToken cancellationToken)
        {
            var tokens = await _registerUserUseCase.ExecuteAsync(request, cancellationToken);

            if (tokens == null)
            {
                return BadRequest();
            }

            Response.Cookies.Append("accessToken", tokens.Value.Item1, _accessTokenCookieOptions);
            Response.Cookies.Append("refreshToken", tokens.Value.Item2, _refreshTokenCookieOptions);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginWithEmailAndPasswordUser(
            [FromForm] LoginRequest request,
            CancellationToken cancellationToken)
        {
            var tokens = await _loginUseCase.ExecuteAsync(request, cancellationToken);

            if (tokens == null)
            {
                return BadRequest();
            }

            Response.Cookies.Append("accessToken", tokens.Value.Item1, _accessTokenCookieOptions);
            Response.Cookies.Append("refreshToken", tokens.Value.Item2, _refreshTokenCookieOptions);

            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokens(
            CancellationToken cancellationToken)
        {
            Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

            if (refreshToken == null)
            {
                return Unauthorized();
            }

            var tokens = await _refreshTokensUseCase.ExecuteAsync(refreshToken, cancellationToken);

            if (tokens == null)
            {
                return BadRequest();
            }

            Response.Cookies.Append("accessToken", tokens.Value.Item1, _accessTokenCookieOptions);
            Response.Cookies.Append("refreshToken", tokens.Value.Item2, _refreshTokenCookieOptions);

            return Ok();
        }
    }
}
