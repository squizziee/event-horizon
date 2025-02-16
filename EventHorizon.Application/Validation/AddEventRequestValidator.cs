using EventHorizon.Contracts.Requests.Events;
using FluentValidation;

namespace EventHorizon.Application.Validation
{
    public class AddEventRequestValidator : AbstractValidator<AddEventRequest>
    {
        public AddEventRequestValidator()
        {
            RuleFor(r => r.Address)
                .Length(1, 150);

            RuleFor(r => r.Name)
                .Length(1, 150);

            RuleFor(r => r.Description)
                .Length(1, 1000);

            RuleFor(r => r.MaxParticipantCount)
                .GreaterThan(0);

            RuleFor(r => r.Date)
                .Must(d => d.CompareTo(DateOnly.FromDateTime(DateTime.Now)) > 0);

            RuleFor(r => r.Time)
                .Must(t => t.CompareTo(TimeOnly.FromDateTime(DateTime.Now)) > 0)
                .When(r => r.Date.CompareTo(DateOnly.FromDateTime(DateTime.Now)) == 0);
        }
    }
}
