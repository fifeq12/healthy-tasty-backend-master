using System.Net;
using HealthyTasty.Domain.Tables;
using HealthyTasty.Infrastructure.Exceptions;
using HealthyTasty.Repositories;
using Microsoft.AspNetCore.Identity;

namespace HealthyTasty.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public IdentityService(IPasswordHasher<User> passwordHasher, IRefreshTokenRepository refreshTokenRepository)
        {
            _passwordHasher = passwordHasher;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public void HashPassword(ref User user, string password)
        {
            user.Password = _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string password)
            => _passwordHasher.VerifyHashedPassword(user, user.Password, password)
               != PasswordVerificationResult.Failed;

        public async Task SaveRefreshToken(string refreshToken, User user, CancellationToken cancellationToken)
        {
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                User = user
            };

            _refreshTokenRepository.Create(refreshTokenEntity);
            await _refreshTokenRepository.SaveChanges(cancellationToken);
        }

        public async Task RevokeUserRefreshTokens(long userId, CancellationToken cancellationToken)
        {
            var userRefreshTokens = await _refreshTokenRepository
                .GetAll(x => x.User.Id == userId && !x.Revoked, cancellationToken);

            foreach (var userRefreshToken in userRefreshTokens)
            {
                userRefreshToken.Revoked = true;
                _refreshTokenRepository.Update(userRefreshToken);
            }

            await _refreshTokenRepository.SaveChanges(cancellationToken);

            throw new ApiException(HttpStatusCode.Forbidden, ErrorCodes.RefreshTokenAlreadyRevoked,
                "Refresh token was revoked.");
        }
    }

    public interface IIdentityService
    {
        void HashPassword(ref User user, string password);
        bool VerifyPassword(User user, string password);
        Task SaveRefreshToken(string refreshToken, User user, CancellationToken cancellationToken);
        Task RevokeUserRefreshTokens(long userId, CancellationToken cancellationToken);
    }
}
