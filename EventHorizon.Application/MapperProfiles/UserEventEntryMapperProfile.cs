using AutoMapper;
using EventHorizon.Contracts.DTO;
using EventHorizon.Domain.Entities;

namespace EventHorizon.Application.MapperProfiles
{
    public class UserEventEntryMapperProfile : Profile
    {
        public UserEventEntryMapperProfile() {
            CreateMap<EventEntry, UserEventEntryDTO>();
        }
    }
}
