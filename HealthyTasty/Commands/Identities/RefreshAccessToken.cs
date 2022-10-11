using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using FluentValidation;
using HealthyTasty.Domain.Tables;
using HealthyTasty.Infrastructure.Exceptions;
using HealthyTasty.Infrastructure.Jwt;
using HealthyTasty.Repositories;
using HealthyTasty.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HealthyTasty.Commands.Identities
{
    public record RefreshAccessToken(string RefreshToken) : IRequest<JsonWebToken>
    {
        public class RegisterValidator : AbstractValidator<RefreshAccessToken>
        {
            public RegisterValidator()
            {
                RuleFor(x => x.RefreshToken).NotEmpty();
            }
        }
    }

    public class RefreshTokenHandler : IRequestHandler<RefreshAccessToken, JsonWebToken>
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IIdentityService _identityService;

        public RefreshTokenHandler(IJwtHelper jwtHelper, IRefreshTokenRepository refreshTokenRepository, IIdentityService identityService)
        {
            _jwtHelper = jwtHelper;
            _refreshTokenRepository = refreshTokenRepository;
            _identityService = identityService;
        }

        public async Task<JsonWebToken> Handle(RefreshAccessToken request, CancellationToken cancellationToken)
        {
            var isRefreshTokenValid = _jwtHelper.VerifyRefreshToken(request.RefreshToken);

            var refreshToken = await _refreshTokenRepository.GetBy(x => x.Token
                .Equals(request.RefreshToken), x => x
                .Include(y => y.User), cancellationToken);

            if (refreshToken is null)
                throw new ApiException(HttpStatusCode.NotFound, ErrorCodes.RefreshTokenNotFound, "Refresh token was not found.");

            if (refreshToken.Revoked || !isRefreshTokenValid)
                await _identityService.RevokeUserRefreshTokens(refreshToken.User.Id, cancellationToken);

            refreshToken.Revoked = true;
            _refreshTokenRepository.Update(refreshToken);
            await _refreshTokenRepository.SaveChanges(cancellationToken);

            var jsonWebToken = _jwtHelper.GenerateAccessToken(refreshToken.User.Id, refreshToken.User.Role);
            var jwtRefreshToken = _jwtHelper.GenerateRefreshToken();

            await _identityService.SaveRefreshToken(jwtRefreshToken, refreshToken.User, cancellationToken);

            jsonWebToken.RefreshToken = jwtRefreshToken;

            return jsonWebToken;
        }
    }
}
