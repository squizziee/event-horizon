using EventHorizon.Contracts.Requests.EventCategories;
using FluentValidation;

namespace EventHorizon.Application.Validation
{
    public class AddCategoryRequestValidator : AbstractValidator<AddCategoryRequest>
    {
        public AddCategoryRequestValidator() {
            RuleFor(r => r.Name)
                .Length(1, 150);

            RuleFor(r => r.Description)
                .Length(0, 1000);
        }
    }
}
