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
        public AuthenticationController(IRegisterUserUseCase registerUserUseCase) {
            _registerUserUseCase = registerUserUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(
            [FromForm] RegsiterUserRequest request, 
            CancellationToken cancellationToken)
        {
            await _registerUserUseCase.ExecuteAsync(request, cancellationToken);
            return Ok(request);
        }
    }
}
