using System.Net;
using FluentValidation;
using HealthyTasty.Domain.Tables;
using HealthyTasty.Infrastructure.Exceptions;
using HealthyTasty.Infrastructure.Jwt;
using HealthyTasty.Repositories;
using HealthyTasty.Services;
using MediatR;

namespace HealthyTasty.Commands.Identities
{
    public record Login(string Username, string Password) : IRequest<JsonWebToken>
    {
        public class RegisterValidator : AbstractValidator<Login>
        {
            public RegisterValidator()
            {
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }
    }

    public class LoginHandler : IRequestHandler<Login, JsonWebToken>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityService;
        private readonly IJwtHelper _jwtHelper;

        public LoginHandler(IUserRepository userRepository, IIdentityService identityService, IJwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _jwtHelper = jwtHelper;
        }

        public async Task<JsonWebToken> Handle(Login request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetBy(x => x.Username.ToUpper()
                .Equals(request.Username.ToUpper()), cancellationToken);

            if (user is null || !_identityService.VerifyPassword(user, request.Password))
                throw new ApiException(HttpStatusCode.Unauthorized, ErrorCodes.InvalidCredentials, "Invalid credentials.");

            var jsonWebToken = _jwtHelper.GenerateAccessToken(user.Id, user.Role);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            await _identityService.SaveRefreshToken(refreshToken, user, cancellationToken);

            jsonWebToken.RefreshToken = refreshToken;

            return jsonWebToken;
        }
    }
}
