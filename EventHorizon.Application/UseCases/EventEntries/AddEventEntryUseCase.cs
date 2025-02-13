using EventHorizon.Application.UseCases.Interfaces.EventEntries;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Domain.Entities;
using EventHorizon.Infrastructure.Data;

namespace EventHorizon.Application.UseCases.EventEntries
{
    public class AddEventEntryUseCase : IAddEventEntryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddEventEntryUseCase(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(Guid userId, Guid eventId, CancellationToken cancellationToken)
        {
            var tryFindUser = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);

            if (tryFindUser == null)
            {
                throw new ResourceNotFoundException($"No user with id {userId} was found");
            }

            var tryFindEvent = await _unitOfWork.Events.GetByIdAsync(eventId, cancellationToken);

            if (tryFindEvent == null)
            {
                throw new ResourceNotFoundException($"No event with id {eventId} was found");
            }

            if (tryFindEvent.Entries.Count >= tryFindEvent.MaxParticipantCount)
            {
                throw new BadRequestException($"Event with id {eventId} has reached maximum entries");
            }

            var newEntry = new EventEntry
            {
                EventId = eventId,
                UserId = userId,
                SubmissionDate = DateTime.UtcNow,
            };

            await _unitOfWork.Entries.AddAsync(newEntry, cancellationToken);
            _unitOfWork.Save();
        }
    }
}
