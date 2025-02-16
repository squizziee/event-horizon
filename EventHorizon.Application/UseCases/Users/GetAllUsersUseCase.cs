using AutoMapper;
using EventHorizon.Application.UseCases.Interfaces.Users;
using EventHorizon.Contracts.DTO;
using EventHorizon.Contracts.Responses;
using EventHorizon.Infrastructure.Data;

namespace EventHorizon.Application.UseCases.Users
{
    public class GetAllUsersUseCase : IGetAllUsersUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetAllUsersResponse> ExecuteAsync(CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetAllAsync(cancellationToken);

            return new GetAllUsersResponse
            {
                Users = users.Select(_mapper.Map<UserDTO>)
            };
        }
    }
}
