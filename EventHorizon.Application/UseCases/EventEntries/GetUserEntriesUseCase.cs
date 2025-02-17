using AutoMapper;
using EventHorizon.Application.UseCases.Interfaces.EventEntries;
using EventHorizon.Contracts.DTO;
using EventHorizon.Contracts.Responses.EventEntries;
using EventHorizon.Domain.Interfaces.Repositories;

namespace EventHorizon.Application.UseCases.EventEntries
{
    public class GetUserEntriesUseCase : IGetUserEntriesUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserEntriesUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetUserEntriesResponse> ExecuteAsync(Guid userId, CancellationToken cancellationToken)
        {
            var entries = await _unitOfWork.Entries.GetByUserIdWithEventAsync(userId, cancellationToken);

            return new GetUserEntriesResponse
            {
                Entries = entries.Select(_mapper.Map<UserEventEntryDTO>)
            };
        }
    }
}
