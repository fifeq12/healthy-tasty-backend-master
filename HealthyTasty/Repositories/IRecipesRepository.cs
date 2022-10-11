using HealthyTasty.Domain;
using HealthyTasty.Domain.Tables;
using HealthyTasty.Infrastructure.Pagination;

namespace HealthyTasty.Repositories
{
    public class RecipesRepository : GenericRepository<Recipe>, IRecipesRepository
    {
        public RecipesRepository(HealthyTastyContext context) : base(context)
        {
        }

        public Task<PagedResult<Recipe>> GetAllRecipesPaged(PagedQueryBase query)
        {
            var result = Context.Recipes.Where(x => true);
            return result.Paginate(query);
        }
    }

    public interface IRecipesRepository : IGenericRepository<Recipe>
    {
        Task<PagedResult<Recipe>> GetAllRecipesPaged(PagedQueryBase query);
    }
}
