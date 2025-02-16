using EventHorizon.Application.UseCases.Interfaces.Dev;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventHorizon.API.Controllers
{
    [Route("api/dev")]
    [ApiController]
    public class DevController : ControllerBase
    {
        private readonly ICreateAdminUseCase _createAdminUseCase;
        private readonly ISeedDatabaseUseCase _seedDatabaseUseCase;

        public DevController(
            ICreateAdminUseCase createAdminUseCase,
            ISeedDatabaseUseCase seedDatabaseUseCase) { 
            _createAdminUseCase = createAdminUseCase;
            _seedDatabaseUseCase = seedDatabaseUseCase;
        }

        [HttpPost("admin")]
        public async Task<IActionResult> CreateAdmin(CancellationToken cancellationToken)
        {
            await _createAdminUseCase.ExecuteAsync(cancellationToken);
            return Ok();
        }

        [HttpPost("seed")]
        public async Task<IActionResult> SeedDatabase(CancellationToken cancellationToken)
        {
            await _seedDatabaseUseCase.ExecuteAsync(cancellationToken);
            return Ok();
        }
    }
}
