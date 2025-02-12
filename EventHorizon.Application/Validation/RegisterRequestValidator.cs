using EventHorizon.Contracts.Requests;
using FluentValidation;

namespace EventHorizon.Application.Validation
{
    public class RegisterRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterRequestValidator() {
            RuleFor(r => r.Email)
                .EmailAddress()
                .WithMessage("Wrong email format");

            RuleFor(r => r.FirstName)
                .Length(1, 50);

            RuleFor(r => r.LastName)
                .Length(2, 150);
        }
    }
}
