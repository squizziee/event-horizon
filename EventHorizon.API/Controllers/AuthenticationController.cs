using EventHorizon.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EventHorizon.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public AuthenticationController() {
            
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegsiterUserRequest request)
        {
            return Ok(request);
        }
    }
}
