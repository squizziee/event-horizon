using AutoMapper;
using EventHorizon.Application.Helpers;
using EventHorizon.Application.UseCases.Interfaces.EventEntries;
using EventHorizon.Contracts.DTO;
using EventHorizon.Contracts.Requests.EventEntries;
using EventHorizon.Contracts.Responses.EventEntries;
using EventHorizon.Infrastructure.Data;
using Microsoft.Extensions.Options;

namespace EventHorizon.Application.UseCases.EventEntries
{
    public class GetEventEntriesUseCase : IGetEventEntriesUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PaginationOptions _paginationOptions;

        public GetEventEntriesUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paginationOptions = options.Value;
        }

        public async Task<GetEventEntriesResponse> ExecuteAsync(
            Guid eventId, 
            GetEventEntriesRequest request, 
            CancellationToken cancellationToken)
        {
            var entries = await _unitOfWork.Entries.GetFilteredAsync(
                ee => ee.EventId == eventId,
                request.PageNumber,
                _paginationOptions.PageSize,
                cancellationToken
            );

            return new GetEventEntriesResponse
            {
                PageNumber = entries.ChunkSequenceNumber,
                TotalPages = entries.TotalChunkCount,
                Entries = entries.Items.Select(_mapper.Map<EventEntryDTO>)
            };
        }
    }
}
