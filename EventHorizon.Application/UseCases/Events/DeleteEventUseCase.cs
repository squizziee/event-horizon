using EventHorizon.Application.UseCases.Interfaces.Events;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Domain.Interfaces.Repositories;
using EventHorizon.Infrastructure.Services.Interfaces;

namespace EventHorizon.Application.UseCases.Events
{
    public class DeleteEventUseCase : IDeleteEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public DeleteEventUseCase(
            IUnitOfWork unitOfWork,
            IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            var tryFind = await _unitOfWork.Events.GetByIdAsync(id, cancellationToken);

            if (tryFind == null)
            {
                throw new ResourceNotFoundException($"No event with id {id} was found");
            }

            foreach (var url in tryFind.ImageUrls)
            {
                try
                {
                    await _imageService.DeleteImage(url);
                }
                catch (ResourceNotFoundException)
                {
                    continue;
                }

            }

            await _unitOfWork.Events.DeleteAsync(tryFind, cancellationToken);
            _unitOfWork.Save();
        }
    }
}
