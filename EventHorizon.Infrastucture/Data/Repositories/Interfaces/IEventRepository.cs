using EventHorizon.Domain.Entities;

namespace EventHorizon.Infrastructure.Data.Repositories.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
    }
}
