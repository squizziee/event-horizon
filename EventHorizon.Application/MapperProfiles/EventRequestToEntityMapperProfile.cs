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
            CreateMap<UpdateEventRequest, Event>()
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom<DateResolver2>());
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
            var date = new DateTime(source.Date, source.Time, DateTimeKind.Utc);
            return date;
        }
    }

    public class DateResolver2 : IValueResolver<UpdateEventRequest, Event, DateTime>
    {

        public DateTime Resolve(
            UpdateEventRequest source,
            Event destination,
            DateTime destMember,
            ResolutionContext context)
        {
            var date = new DateTime(source.Date, source.Time, DateTimeKind.Utc);
            return date;
        }
    }
}
