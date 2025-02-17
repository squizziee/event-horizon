using AutoMapper;
using EventHorizon.Application.UseCases.Interfaces.EventEntries;
using EventHorizon.Contracts.DTO;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Contracts.Responses.EventEntries;
using EventHorizon.Domain.Interfaces.Repositories;

namespace EventHorizon.Application.UseCases.EventEntries
{
    public class GetEventEntryUseCase : IGetEventEntryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventEntryUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetEntryResponse> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            var tryFindEntry = await _unitOfWork.Entries.GetByIdAsync(id, cancellationToken);

            if (tryFindEntry == null)
            {
                throw new ResourceNotFoundException($"No event with id {id} was found");
            }

            return new GetEntryResponse
            {
                Entry = _mapper.Map<EventEntryDTO>(tryFindEntry),
            };
        }
    }
}
