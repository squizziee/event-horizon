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
                .Length(0, 1000);

            RuleFor(r => r.MaxParticipantCount)
                .GreaterThan(0);
        }
    }
}
