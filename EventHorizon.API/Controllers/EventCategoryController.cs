using EventHorizon.Application.UseCases.Interfaces.EventCategories;
using EventHorizon.Contracts.Requests.EventCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventHorizon.API.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class EventCategoryController : ControllerBase
    {
        private readonly IGetAllCategoriesUseCase _getAllCategoriesUseCase;
        private readonly IGetCategoryUseCase _getCategoryUseCase;
        private readonly IAddCategoryUseCase _addCategoryUseCase;
        private readonly IUpdateCategoryUseCase _updateCategoryUseCase;
        private readonly IDeleteCategoryUseCase _deleteCategoryUseCase;

        public EventCategoryController(
            IGetAllCategoriesUseCase getAllCategoriesUseCase,
            IGetCategoryUseCase getCategoryUseCase,
            IAddCategoryUseCase addCategoryUseCase,
            IUpdateCategoryUseCase updateCategoryUseCase,
            IDeleteCategoryUseCase deleteCategoryUseCase) {
            _getAllCategoriesUseCase = getAllCategoriesUseCase;
            _getCategoryUseCase = getCategoryUseCase;
            _addCategoryUseCase = addCategoryUseCase;
            _updateCategoryUseCase = updateCategoryUseCase;
            _deleteCategoryUseCase = deleteCategoryUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories(
            [FromQuery] GetAllCategoriesRequest request,
            CancellationToken cancellationToken)
        {
            var categories = await _getAllCategoriesUseCase.ExecuteAsync(request, cancellationToken);

            return Ok(categories);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCategory(
            [FromRoute] Guid Id,
            CancellationToken cancellationToken)
        {
            var category = await _getCategoryUseCase.ExecuteAsync(Id, cancellationToken);

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddCategory(
            [FromForm] AddCategoryRequest request,
            CancellationToken cancellationToken)
        {
            await _addCategoryUseCase.ExecuteAsync(request, cancellationToken);
            return Ok();
        }

        [HttpPut("{Id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateCategory(
            [FromRoute] Guid Id,
            [FromForm] UpdateCategoryRequest request,
            CancellationToken cancellationToken)
        {
            await _updateCategoryUseCase.ExecuteAsync(Id, request, cancellationToken);
            return Ok();
        }

        [HttpDelete("{Id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteCategory(
            [FromRoute] Guid Id,
            CancellationToken cancellationToken)
        {
            await _deleteCategoryUseCase.ExecuteAsync(Id, cancellationToken);
            return Ok();
        }

    }
}
