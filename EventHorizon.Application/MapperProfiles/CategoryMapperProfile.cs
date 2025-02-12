using AutoMapper;
using EventHorizon.Contracts.DTO;
using EventHorizon.Domain.Entities;

namespace EventHorizon.Application.MapperProfiles
{
    public class CategoryMapperProfile : Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<EventCategory, EventCategoryDTO>();
        }
    }
}
