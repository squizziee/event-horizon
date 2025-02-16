using EventHorizon.Contracts.Requests.EventCategories;
using FluentValidation;

namespace EventHorizon.Application.Validation
{
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(r => r.Name)
                .Length(1, 150)
                .WithMessage("Name should be between 1 and 150 symbols long");

            RuleFor(r => r.Description)
                .Length(1, 1000)
                .WithMessage("Description should be between 1 and 1000 symbols long");
        }
    }
}
