using HealthyTasty.Domain;
using HealthyTasty.Domain.Tables;

namespace HealthyTasty.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(HealthyTastyContext context) : base(context)
        {
        }
    }

    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
    }
}
