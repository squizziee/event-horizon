using EventHorizon.Contracts.Requests.Events;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHorizon.Application.Validation
{
    public class UpdateEventRequestValidator : AbstractValidator<UpdateEventRequest>
    {
        public UpdateEventRequestValidator()
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
