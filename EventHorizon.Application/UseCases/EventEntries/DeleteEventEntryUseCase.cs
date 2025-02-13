using AutoMapper;
using EventHorizon.Application.UseCases.Interfaces.EventEntries;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Infrastructure.Data;

namespace EventHorizon.Application.UseCases.EventEntries
{
    public class DeleteEventEntryUseCase : IDeleteEventEntryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEventEntryUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            var tryFind = await _unitOfWork.Entries.GetByIdAsync(id, cancellationToken);

            if (tryFind == null)
            {
                throw new ResourceNotFoundException($"No event entry with id {id} was found");
            }

            await _unitOfWork.Entries.DeleteAsync(tryFind, cancellationToken);
            _unitOfWork.Save();
        }
    }
}
