using EventHorizon.Application.UseCases.Interfaces.EventCategories;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Infrastructure.Data;

namespace EventHorizon.Application.UseCases.EventCategories
{
    public class DeleteCategoryUseCase : IDeleteCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryUseCase(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            var tryFind = await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);

            if (tryFind == null)
            {
                throw new ResourceNotFoundException($"No category with id {id} was found");
            }

            await _unitOfWork.Categories.DeleteAsync(tryFind, cancellationToken);
            _unitOfWork.Save();
        }
    }
}
