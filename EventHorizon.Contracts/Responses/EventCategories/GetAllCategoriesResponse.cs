using EventHorizon.Contracts.DTO;

namespace EventHorizon.Contracts.Responses.EventCategories
{
    public record GetAllCategoriesResponse
    {
        public required IEnumerable<EventCategoryDTO> Categories { get; set; }
        public required int PageNumber { get; set; }
        public required int TotalPages { get; set; }
    }
}
