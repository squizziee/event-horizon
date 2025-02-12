using EventHorizon.Contracts.DTO;

namespace EventHorizon.Contracts.Responses.EventCategories
{
    public class GetCategoryResponse
    {
        public required EventCategoryDTO Category { get; set; }
    }
}
