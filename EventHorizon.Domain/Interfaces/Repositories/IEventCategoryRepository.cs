using EventHorizon.Domain.Entities;

namespace EventHorizon.Domain.Interfaces.Repositories
{
    public interface IEventCategoryRepository : IRepository<EventCategory>, IPaginatableRepository<EventCategory>
    {

    }
}
