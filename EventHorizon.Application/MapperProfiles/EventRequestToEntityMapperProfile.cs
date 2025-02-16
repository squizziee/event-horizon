using AutoMapper;
using EventHorizon.Contracts.Requests.Events;
using EventHorizon.Domain.Entities;

namespace EventHorizon.Application.MapperProfiles
{
    public class EventRequestToEntityMapperProfile : Profile
    {
        public EventRequestToEntityMapperProfile()
        {
            CreateMap<AddEventRequest, Event>()
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom<DateResolver>());
            CreateMap<UpdateEventRequest, Event>();
        }
    }

    public class DateResolver : IValueResolver<AddEventRequest, Event, DateTime>
    {

        public DateTime Resolve(
            AddEventRequest source, 
            Event destination, 
            DateTime destMember, 
            ResolutionContext context)
        {
            return new DateTime(source.Date, source.Time, DateTimeKind.Utc);
        }
    }
}
