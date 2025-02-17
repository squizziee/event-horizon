using AutoMapper;
using EventHorizon.Application.UseCases.Interfaces.Events;
using EventHorizon.Contracts.DTO;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Contracts.Responses.Events;
using EventHorizon.Domain.Interfaces.Repositories;

namespace EventHorizon.Application.UseCases.Events
{
    public class GetEventUseCase : IGetEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetOneEventResponse> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            var tryFind = await _unitOfWork.Events.GetByIdAsync(id, cancellationToken);

            if (tryFind == null)
            {
                throw new ResourceNotFoundException($"No event with id {id} was found");
            }

            return new GetOneEventResponse
            {
                Event = _mapper.Map<EventDTO>(tryFind)
            };
        }
    }
}
