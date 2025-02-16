using EventHorizon.Application.UseCases.Interfaces.EventCategories;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Contracts.Requests.EventCategories;
using EventHorizon.Domain.Entities;
using EventHorizon.Infrastructure.Data;
using FluentValidation;

namespace EventHorizon.Application.UseCases.EventCategories
{
    public class AddCategoryUseCase : IAddCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddCategoryRequest> _validator;

        public AddCategoryUseCase(
            IUnitOfWork unitOfWork,
            IValidator<AddCategoryRequest> validator) { 
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task ExecuteAsync(AddCategoryRequest request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            var newCategory = new EventCategory
            {
                Name = request.Name,
                Description = request.Description,
            };

            await _unitOfWork.Categories.AddAsync(newCategory, cancellationToken);
            _unitOfWork.Save();
        }
    }
}
