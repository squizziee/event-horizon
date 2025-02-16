using EventHorizon.Contracts.Requests.Events;
using FluentValidation;

namespace EventHorizon.Application.Validation
{
    public class UpdateEventRequestValidator : AbstractValidator<UpdateEventRequest>
    {
        public UpdateEventRequestValidator()
        {
            RuleFor(r => r.Address)
                 .Length(1, 150)
                 .WithMessage("Address should be between 1 and 150 symbols long");

            RuleFor(r => r.Name)
                .Length(1, 150)
                .WithMessage("Name should be between 1 and 150 symbols long");

            RuleFor(r => r.Description)
                .Length(1, 1000)
                .WithMessage("Description should be between 1 and 1000 symbols long");

            RuleFor(r => r.MaxParticipantCount)
                .GreaterThan(0)
                .WithMessage(_ => "Event should have at least one participant");

            RuleFor(r => r.Date)
                .Must(d => d.CompareTo(DateOnly.FromDateTime(DateTime.Now)) > 0)
                .WithMessage("Can't schedule event in the past");

            RuleFor(r => r.Time)
                .Must(t => t.CompareTo(TimeOnly.FromDateTime(DateTime.Now)) > 0)
                .When(r => r.Date.CompareTo(DateOnly.FromDateTime(DateTime.Now)) == 0)
                .WithMessage("Can't schedule event in the past");

        }
    }
}
