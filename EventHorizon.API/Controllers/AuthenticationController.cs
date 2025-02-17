using EventHorizon.Application.UseCases.Interfaces.Users;
using EventHorizon.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventHorizon.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IRegisterUserUseCase _registerUserUseCase;
        private readonly ILoginUseCase _loginUseCase;
        private readonly IRefreshTokensUseCase _refreshTokensUseCase;
        private readonly IGetUserDataUseCase _getUserDataUseCase;
        private readonly IGetAllUsersUseCase _getAllUsersUseCase;
        private readonly CookieOptions _accessTokenCookieOptions;
        private readonly CookieOptions _refreshTokenCookieOptions;
        public AuthenticationController(
            IRegisterUserUseCase registerUserUseCase,
            ILoginUseCase loginUseCase,
            IRefreshTokensUseCase refreshTokensUseCase,
            IGetUserDataUseCase getUserDataUseCase,
            IGetAllUsersUseCase getAllUsersUseCase,
            IConfiguration configuration) {
            _registerUserUseCase = registerUserUseCase;
            _loginUseCase = loginUseCase;
            _refreshTokensUseCase = refreshTokensUseCase;
            _getUserDataUseCase = getUserDataUseCase;
            _getAllUsersUseCase = getAllUsersUseCase;

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
            [FromForm] RegisterUserRequest request, 
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

        [HttpPost("logout")]
        [Authorize(Policy = "ViewerPolicy")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Append("accessToken", "");
            Response.Cookies.Append("refreshToken", "");

            return Ok();
        }

        [HttpPost("refresh")]
        [Authorize(Policy = "ViewerPolicy")]
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

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserData(
            [FromRoute] Guid Id,
            CancellationToken cancellationToken)
        {
            var data = await _getUserDataUseCase.ExecuteAsync(Id, cancellationToken);

            return Ok(data);
        }

        [HttpGet("me")]
        [Authorize(Policy = "ViewerPolicy")]
        public async Task<IActionResult> GetMe(
            CancellationToken cancellationToken)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);

            if (userId == null)
            {
                return Unauthorized();
            }

            var data = await _getUserDataUseCase.ExecuteAsync(Guid.Parse(userId.Value), cancellationToken);

            return Ok(data);
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllUsers(
            CancellationToken cancellationToken)
        {
            var users = await _getAllUsersUseCase.ExecuteAsync(cancellationToken);

            return Ok(users);
        }
    }
}
