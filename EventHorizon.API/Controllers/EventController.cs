using EventHorizon.Application.UseCases.Interfaces.Events;
using EventHorizon.Contracts.Requests.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventHorizon.API.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IGetAllEventsUseCase _getAllEventsUseCase;
        private readonly IGetEventUseCase _getEventUseCase;
        private readonly ISearchEventsUseCase _searchEventsUseCase;
        private readonly IAddEventUseCase _addEventUseCase;
        private readonly IUpdateEventUseCase _updateEventUseCase;
        private readonly IDeleteEventUseCase _deleteEventUseCase;


        public EventController(
            IGetAllEventsUseCase getAllEventsUseCase,
            IGetEventUseCase getEventUseCase,
            ISearchEventsUseCase searchEventsUseCase,
            IAddEventUseCase addEventUseCase,
            IUpdateEventUseCase updateEventUseCase,
            IDeleteEventUseCase deleteEventUseCase) { 
            _getAllEventsUseCase = getAllEventsUseCase;
            _getEventUseCase = getEventUseCase;
            _searchEventsUseCase = searchEventsUseCase;
            _addEventUseCase = addEventUseCase;
            _updateEventUseCase = updateEventUseCase;
            _deleteEventUseCase = deleteEventUseCase;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents(
            [FromQuery] GetAllEventsRequest request,
            CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetEvent(
            [FromQuery] Guid Id,
            CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchEvents(
            [FromQuery] SearchEventsRequest request,
            CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpPost]
        [Authorize(Policy = "ViewerPolicy")]
        public async Task<IActionResult> AddEvent(
            [FromQuery] AddEventRequest request,
            CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpPut("{Id}")]
        [Authorize(Policy = "ViewerPolicy")]
        public async Task<IActionResult> UpdateEvent(
            [FromRoute] Guid Id,
            [FromQuery] UpdateEventRequest request,
            CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete("{Id}")]
        [Authorize(Policy = "ViewerPolicy")]
        public async Task<IActionResult> DeleteEvent(
            [FromRoute] Guid Id,
            CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
