using AutoMapper;
using EventHorizon.Application.Helpers;
using EventHorizon.Application.UseCases.Interfaces.EventCategories;
using EventHorizon.Contracts.DTO;
using EventHorizon.Contracts.Requests.EventCategories;
using EventHorizon.Contracts.Responses.EventCategories;
using EventHorizon.Domain.Interfaces.Repositories;

using Microsoft.Extensions.Options;

namespace EventHorizon.Application.UseCases.EventCategories
{
    public class GetAllCategoriesUseCase : IGetAllCategoriesUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;
        private readonly IMapper _mapper;

        public GetAllCategoriesUseCase(
            IUnitOfWork unitOfWork,
            IOptions<PaginationOptions> options,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
            _mapper = mapper;
        }

        public async Task<GetAllCategoriesResponse> ExecuteAsync(GetAllCategoriesRequest request, CancellationToken cancellationToken)
        {
            if (request.DoNotPaginate)
            {
                var npcategories = await _unitOfWork.Categories.GetAllAsync(cancellationToken);

                return new GetAllCategoriesResponse
                {
                    PageNumber = -1,
                    TotalPages = -1,
                    Categories = npcategories.Select(_mapper.Map<EventCategoryDTO>)
                };
            }

            var categories = await _unitOfWork.Categories.GetAllAsync(
                request.PageNumber,
                _paginationOptions.PageSize,
                cancellationToken
            );

            return new GetAllCategoriesResponse
            {
                PageNumber = categories.ChunkSequenceNumber,
                TotalPages = categories.TotalChunkCount,
                Categories = categories.Items.Select(_mapper.Map<EventCategoryDTO>)
            };
        }
    }
}
