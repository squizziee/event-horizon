﻿using EventHorizon.Application.UseCases.Interfaces.Events;
using EventHorizon.Contracts.Requests.Events;
using Microsoft.AspNetCore.Authorization;
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
            var events = await _getAllEventsUseCase.ExecuteAsync(request, cancellationToken);

            var list = events.Events.ToList();

            return Ok(events);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetEvent(
            [FromRoute] Guid Id,
            CancellationToken cancellationToken)
        {
            var data = await _getEventUseCase.ExecuteAsync(Id, cancellationToken);

            return Ok(data);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchEvents(
            [FromQuery] SearchEventsRequest request,
            CancellationToken cancellationToken)
        {
            var data = await _searchEventsUseCase.ExecuteAsync(request, cancellationToken);

            return Ok(data);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddEvent(
            [FromForm] AddEventRequest request,
            CancellationToken cancellationToken)
        {
            await _addEventUseCase.ExecuteAsync(request, cancellationToken);

            return Ok();
        }

        [HttpPut("{Id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateEvent(
            [FromRoute] Guid Id,
            [FromForm] UpdateEventRequest request,
            CancellationToken cancellationToken)
        {
            await _updateEventUseCase.ExecuteAsync(Id, request, cancellationToken);

            return Ok();
        }

        [HttpDelete("{Id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteEvent(
            [FromRoute] Guid Id,
            CancellationToken cancellationToken)
        {
            await _deleteEventUseCase.ExecuteAsync(Id, cancellationToken);

            return Ok();
        }
    }
}
