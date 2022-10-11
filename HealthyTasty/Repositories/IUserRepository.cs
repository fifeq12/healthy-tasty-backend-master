using HealthyTasty.Domain;
using HealthyTasty.Domain.Tables;
using Microsoft.EntityFrameworkCore;

namespace HealthyTasty.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(HealthyTastyContext context) : base(context)
        {
        }

        public async Task<bool> IsSameUserAlreadyExist(long? userId, string username, string email, CancellationToken cancellationToken)
        {
            var result = Context.Users.Where(x => x.Username.ToUpper() == username.ToUpper() 
                                                  || x.Email == email);
            if (userId != null)
                result = result.Where(x => x.Id != userId);

            return await result.AnyAsync(cancellationToken);
        }
    }

    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> IsSameUserAlreadyExist(long? userId, string username, string email, CancellationToken cancellationToken);
    }
}
