using AutoMapper;
using EventHorizon.Contracts.DTO;
using EventHorizon.Domain.Entities;

namespace EventHorizon.Application.MapperProfiles
{
    public class EventMapperProfile : Profile
    {
        public EventMapperProfile()
        {
            CreateMap<Event, EventDTO>()
                .ForMember(
                    dest => dest.CurrentParticipantCount,
                    opt => opt.MapFrom(src => src.Entries.Count())
                );
        }
    }
}
