using EventHorizon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHorizon.Infrastructure.Data.Repositories.Interfaces
{
    public interface IEventEntryRepository : IRepository<EventEntry>
    {
    }
}
