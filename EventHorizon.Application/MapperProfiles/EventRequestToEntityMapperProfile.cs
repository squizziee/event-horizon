using AutoMapper;
using EventHorizon.Contracts.Requests.Events;
using EventHorizon.Domain.Entities;

namespace EventHorizon.Application.MapperProfiles
{
    public class EventRequestToEntityMapperProfile : Profile
    {
        public EventRequestToEntityMapperProfile()
        {
            CreateMap<AddEventRequest, Event>();
            CreateMap<UpdateEventRequest, Event>();
        }
    }
}
