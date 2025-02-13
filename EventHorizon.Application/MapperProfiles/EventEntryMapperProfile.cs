using AutoMapper;
using EventHorizon.Contracts.DTO;
using EventHorizon.Domain.Entities;

namespace EventHorizon.Application.MapperProfiles
{
    public class EventEntryMapperProfile : Profile
    {
        public EventEntryMapperProfile()
        {
            CreateMap<EventEntry, EventEntryDTO>();
        }
    }
}
