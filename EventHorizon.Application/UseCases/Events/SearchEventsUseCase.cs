using AutoMapper;
using EventHorizon.Application.Helpers;
using EventHorizon.Application.UseCases.Interfaces.Events;
using EventHorizon.Contracts.DTO;
using EventHorizon.Contracts.Requests.Events;
using EventHorizon.Contracts.Responses.Events;
using EventHorizon.Domain.Interfaces.Repositories;

using Microsoft.Extensions.Options;

namespace EventHorizon.Application.UseCases.Events
{
    public class SearchEventsUseCase : ISearchEventsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly PaginationOptions _paginationOptions;
        public SearchEventsUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<PaginationOptions> paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paginationOptions = paginationOptions.Value;
        }

        public async Task<GetEventsResponse> ExecuteAsync(SearchEventsRequest request, CancellationToken cancellationToken)
        {
            request.TextQuery ??= string.Empty;
            request.PlaceQuery ??= string.Empty;

            // no operator ??= for DateOnly :)
            if (request.SearchFromDate == null)
            {
                request.SearchFromDate = DateOnly.MinValue;
            }

            if (request.SearchUntilDate == null)
            {
                request.SearchUntilDate = DateOnly.MaxValue;
            }

            var result = await _unitOfWork.Events.GetFilteredAsync(
                e =>
                    (e.Name.Contains(request.TextQuery) || e.Description.Contains(request.TextQuery)) &&
                    e.Address.Contains(request.PlaceQuery) &&
                    e.DateTime.CompareTo(request.SearchFromDate.Value.ToDateTime(TimeOnly.Parse("00:00 AM"))) > 0 &&
                    e.DateTime.CompareTo(request.SearchUntilDate.Value.ToDateTime(TimeOnly.Parse("00:00 AM"))) < 0 &&
                    (request.Categories == null || request.Categories.FirstOrDefault() == null ? true : request.Categories.Contains(e.CategoryId)),
                request.PageNumber,
                _paginationOptions.PageSize,
                cancellationToken
            );

            return new GetEventsResponse
            {
                PageNumber = result.ChunkSequenceNumber,
                TotalPages = result.TotalChunkCount,
                Events = result.Items.Select(_mapper.Map<EventDTO>),
            };

        }
    }
}
