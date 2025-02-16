using EventHorizon.Application.UseCases.Interfaces.EventCategories;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Contracts.Requests.EventCategories;
using EventHorizon.Infrastructure.Data;
using FluentValidation;

namespace EventHorizon.Application.UseCases.EventCategories
{
    public class UpdateCategoryUseCase : IUpdateCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateCategoryRequest> _validator;

        public UpdateCategoryUseCase(
            IUnitOfWork unitOfWork,
            IValidator<UpdateCategoryRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task ExecuteAsync(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            var tryFind = await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);

            if (tryFind == null)
            {
                throw new ResourceNotFoundException($"No category with id {id} was found");
            }

            tryFind.Name = request.Name;
            tryFind.Description = request.Description;

            await _unitOfWork.Categories.UpdateAsync(tryFind, cancellationToken);
            _unitOfWork.Save();
        }
    }
}
