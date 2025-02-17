using AutoMapper;
using EventHorizon.Application.UseCases.Interfaces.EventCategories;
using EventHorizon.Contracts.DTO;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Contracts.Responses.EventCategories;
using EventHorizon.Domain.Interfaces.Repositories;

namespace EventHorizon.Application.UseCases.EventCategories
{
    public class GetCategoryUseCase : IGetCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoryUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetCategoryResponse> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);

            if (category == null)
            {
                throw new ResourceNotFoundException($"No category with id {id} was found");
            }

            return new GetCategoryResponse
            {
                Category = _mapper.Map<EventCategoryDTO>(category),
            };
        }
    }
}
