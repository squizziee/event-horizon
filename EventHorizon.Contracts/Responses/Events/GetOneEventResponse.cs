using EventHorizon.Contracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHorizon.Contracts.Responses.Events
{
    public record class GetOneEventResponse
    {
        public required EventDTO Event { get; set; }
    }
}
