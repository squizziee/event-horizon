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
    public class GetAllEventsUseCase : IGetAllEventsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly PaginationOptions _paginationOptions;

        public GetAllEventsUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<PaginationOptions> paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paginationOptions = paginationOptions.Value;
        }

        public async Task<GetEventsResponse> ExecuteAsync(GetAllEventsRequest request, CancellationToken cancellationToken)
        {
            var events = await _unitOfWork.Events.GetAllAsync(
                request.PageNumber, 
                _paginationOptions.PageSize, 
                cancellationToken
            );

            return new GetEventsResponse
            {
                PageNumber = events.ChunkSequenceNumber,
                TotalPages = events.TotalChunkCount,
                Events = events.Items.Select(_mapper.Map<EventDTO>)
            };
        }
    }
}
