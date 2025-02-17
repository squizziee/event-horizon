using EventHorizon.Application.UseCases.Interfaces.EventEntries;
using EventHorizon.Contracts.Requests.EventEntries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventHorizon.API.Controllers
{
    [Route("api/entries")]
    [ApiController]
    public class EventEntryController : ControllerBase
    {
        private readonly IGetEventEntryUseCase _getEventEntryUseCase;
        private readonly IGetEventEntriesUseCase _getEventEntriesUseCase;
        private readonly IGetUserEntriesUseCase _getUserEntriesUseCase;
        private readonly IAddEventEntryUseCase _addEventEntryUseCase;
        private readonly IDeleteEventEntryUseCase _deleteEventEntryUseCase;

        public EventEntryController(
            IGetEventEntryUseCase getEventEntryUseCase,
            IGetEventEntriesUseCase getEventEntriesUseCase,
            IGetUserEntriesUseCase getUserEntriesUseCase,
            IAddEventEntryUseCase addEventEntryUseCase,
            IDeleteEventEntryUseCase deleteEventEntryUseCase) {
            _getEventEntryUseCase = getEventEntryUseCase;
            _getEventEntriesUseCase = getEventEntriesUseCase;
            _getUserEntriesUseCase = getUserEntriesUseCase;
            _addEventEntryUseCase = addEventEntryUseCase;
            _deleteEventEntryUseCase = deleteEventEntryUseCase;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetEntry(
            [FromRoute] Guid Id,
            CancellationToken cancellationToken)
        {
            var result = await _getEventEntryUseCase.ExecuteAsync(Id, cancellationToken);

            return Ok(result);
        }

        [HttpGet("event/{EventId}")]
        public async Task<IActionResult> GetEventEntries(
            [FromRoute] Guid EventId,
            [FromQuery] GetEventEntriesRequest request,
            CancellationToken cancellationToken)
        {
            var entries = await _getEventEntriesUseCase.ExecuteAsync(EventId, request, cancellationToken);

            return Ok(entries);
        }

        [HttpGet("event/user")]
        [Authorize(Policy = "ViewerPolicy")]
        public async Task<IActionResult> GetUserEventEntries(
            CancellationToken cancellationToken)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);

            if (userId == null)
            {
                return Unauthorized();
            }

            var entries = await _getUserEntriesUseCase.ExecuteAsync(Guid.Parse(userId.Value), cancellationToken);

            return Ok(entries);
        }

        [HttpPost("event/{Id}")]
        [Authorize(Policy = "ViewerPolicy")]
        public async Task<IActionResult> AddEventEntry(
            [FromRoute] Guid Id,
            CancellationToken cancellationToken)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.Name);

            if (userId == null)
            {
                return Unauthorized();
            }

            await _addEventEntryUseCase.ExecuteAsync(Guid.Parse(userId.Value), Id, cancellationToken);

            return Ok();
        }

        [HttpDelete("{Id}")]
        [Authorize(Policy = "ViewerPolicy")]
        public async Task<IActionResult> DeleteEventEntry(
            [FromRoute] Guid Id,
            CancellationToken cancellationToken)
        {
            await _deleteEventEntryUseCase.ExecuteAsync(Id, cancellationToken);

            return Ok();
        }
    }
}
