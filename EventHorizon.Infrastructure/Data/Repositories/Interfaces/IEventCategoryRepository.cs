using EventHorizon.Domain.Entities;

namespace EventHorizon.Infrastructure.Data.Repositories.Interfaces
{
    public interface IEventCategoryRepository : IRepository<EventCategory>, IPaginatableRepository<EventCategory>
    {

    }
}
