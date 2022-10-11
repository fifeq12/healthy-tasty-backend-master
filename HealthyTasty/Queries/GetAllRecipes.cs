using System.Linq.Expressions;
using AutoMapper;
using HealthyTasty.Domain;
using HealthyTasty.Domain.Tables;
using HealthyTasty.Dto;
using HealthyTasty.Infrastructure.Pagination;
using HealthyTasty.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthyTasty.Queries
{
    public class GetAllRecipes : PagedQueryBase, IRequest<PagedResult<BaseRecipeDto>>
    {
    }

    public class GetAllRecipesHandler : IRequestHandler<GetAllRecipes, PagedResult<BaseRecipeDto>>
    {
        private readonly IRecipesRepository _recipesRepository;
        private readonly IMapper _mapper;

        public GetAllRecipesHandler(IRecipesRepository recipesRepository, IMapper mapper)
        {
            _recipesRepository = recipesRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<BaseRecipeDto>> Handle(GetAllRecipes request, CancellationToken cancellationToken)
        {
            var result = await _recipesRepository.GetAllRecipesPaged(request);

            return PagedResult<BaseRecipeDto>.Convert(result, result.Items
                .Select(x => _mapper.Map<BaseRecipeDto>(x)));
        }

    }
}
