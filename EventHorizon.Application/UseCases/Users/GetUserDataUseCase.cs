using AutoMapper;
using EventHorizon.Application.UseCases.Interfaces.Users;
using EventHorizon.Contracts.DTO;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Contracts.Responses;
using EventHorizon.Domain.Interfaces.Repositories;


namespace EventHorizon.Application.UseCases.Users
{
    public class GetUserDataUseCase : IGetUserDataUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserDataUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetUserDataResponse> ExecuteAsync(Guid Id, CancellationToken cancellationToken)
        {
            var tryFind = await _unitOfWork.Users.GetByIdAsync(Id, cancellationToken);

            if (tryFind == null)
            {
                throw new ResourceNotFoundException($"No user with id {Id} was found");
            }

            return new GetUserDataResponse
            {
                User = _mapper.Map<UserDTO>(tryFind),
            };
        }
    }
}
