using EventHorizon.Domain.Entities;

namespace EventHorizon.Domain.Interfaces.Repositories
{
    public interface IEventRepository : IRepository<Event>, IPaginatableRepository<Event>
    {
    }
}
