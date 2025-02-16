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
                .Length(1, 50)
                .WithMessage("First name should be between 1 and 50 symbols long");

            RuleFor(r => r.LastName)
                .Length(2, 150)
                .WithMessage("Last name should be between 2 and 150 symbols long");
        }
    }
}
