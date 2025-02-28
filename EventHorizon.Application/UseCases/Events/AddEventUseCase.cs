﻿using AutoMapper;
using EventHorizon.Application.UseCases.Interfaces.Events;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Contracts.Requests.Events;
using EventHorizon.Domain.Entities;
using EventHorizon.Domain.Interfaces.Repositories;
using EventHorizon.Infrastructure.Services.Interfaces;
using FluentValidation;

namespace EventHorizon.Application.UseCases.Events
{
    public class AddEventUseCase : IAddEventUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IImageService _imageService;
		private readonly IValidator<AddEventRequest> _validator;
		private readonly IMapper _mapper;

		public AddEventUseCase(
			IUnitOfWork unitOfWork,
			IImageService imageService,
			IValidator<AddEventRequest> validator,
			IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_imageService = imageService;
			_validator = validator;
			_mapper = mapper;
		}

		public async Task ExecuteAsync(AddEventRequest request, CancellationToken cancellationToken)
		{
			_validator.ValidateAndThrow(request);

            var tryFindCategory = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId, cancellationToken);

			if (tryFindCategory == null)
			{
				throw new ResourceNotFoundException($"No category with id {request.CategoryId} was found");
			}

			var imageUrls = new List<string>();

			if (request.AttachedImages != null) {
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

			var newEvent = _mapper.Map<Event>(request);
			newEvent.ImageUrls = imageUrls;

			await _unitOfWork.Events.AddAsync(newEvent, cancellationToken);
			_unitOfWork.Save();
		}
	}
}
