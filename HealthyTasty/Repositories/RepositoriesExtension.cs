using System.Linq.Expressions;
using HealthyTasty.Domain;
using Microsoft.EntityFrameworkCore;

namespace HealthyTasty.Repositories
{
    public static class RepositoryExtension
    {
        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRecipesRepository, RecipesRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        }
    }
}
