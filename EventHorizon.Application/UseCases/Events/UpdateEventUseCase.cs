using AutoMapper;
using EventHorizon.Application.UseCases.Interfaces.Events;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Contracts.Requests.Events;
using EventHorizon.Domain.Entities;
using EventHorizon.Infrastructure.Data;
using EventHorizon.Infrastructure.Services.Interfaces;
using FluentValidation;

namespace EventHorizon.Application.UseCases.Events
{
    public class UpdateEventUseCase : IUpdateEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IValidator<UpdateEventRequest> _validator;
        private readonly IMapper _mapper;

        public UpdateEventUseCase(
            IUnitOfWork unitOfWork,
            IImageService imageService,
            IValidator<UpdateEventRequest> validator,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(Guid id, UpdateEventRequest request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException();
            }

            var tryFind = await _unitOfWork.Events.GetByIdAsync(id, cancellationToken);

            if (tryFind == null)
            {
                throw new ResourceNotFoundException($"No event with id {id} was found");
            }

            if (tryFind.Entries.Count() > request.MaxParticipantCount)
            {
                throw new BadRequestException("Can't decrease maximum participant count below actual entry count");
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

            var imageUrls = new List<string>();

            if (request.AttachedImages != null)
            {
                foreach (var file in request.AttachedImages)
                {
                    try
                    {
                        imageUrls.Add(await _imageService.UploadImage(file));
                    }
                    catch (UnsupportedExtensionException)
                    {
                        continue;
                    }

                }
            }
            var tmp = tryFind.Id;
            tryFind = _mapper.Map<Event>(request);

            tryFind.Id = tmp;
            tryFind.ImageUrls = imageUrls;

            await _unitOfWork.Events.UpdateAsync(tryFind, cancellationToken);
            _unitOfWork.Save();
        }
    }
}
