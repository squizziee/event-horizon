using AutoMapper;
using EventHorizon.Application.Helpers;
using EventHorizon.Application.UseCases.Interfaces.Events;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Contracts.Requests.Events;
using EventHorizon.Domain.Entities;
using EventHorizon.Infrastructure.Data;
using FluentValidation;

namespace EventHorizon.Application.UseCases.Events
{
    public class AddEventUseCase : IAddEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddEventRequest> _validator;

        public AddEventUseCase(
            IUnitOfWork unitOfWork,
            IValidator<AddEventRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public Task ExecuteAsync(AddEventRequest request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException();
            }

            var newEvent = new Event
            {
                Name = request.Name,
                Description = request.Description,
                Address = request.Address,
                DateTime = request.DateTime,
                MaxParticipantCount = request.MaxParticipantCount,
                CategoryId = request.CategoryId,
            };
        }
    }
}
