using System.ComponentModel.DataAnnotations;
using System.Net;
using FluentValidation;
using FluentValidation.Validators;
using HealthyTasty.Domain.Tables;
using HealthyTasty.Infrastructure.Enums;
using HealthyTasty.Infrastructure.Exceptions;
using HealthyTasty.Repositories;
using HealthyTasty.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HealthyTasty.Commands.Identities
{
    public record Register(string Username, [EmailAddress] string Email, string Password, string FirstName, 
        string LastName, string Phone) : IRequest
    {
        public class RegisterValidator : AbstractValidator<Register>
        {
            public RegisterValidator()
            {
                RuleFor(x => x.Username).Length(6, 20);
                RuleFor(x => x.Email).EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
                RuleFor(x => x.Phone).NotEmpty();
            }
        }
    }

    public class RegisterHandler : IRequestHandler<Register>
    {
        private readonly IIdentityService _identityService;
        private readonly IUserRepository _identitiesRepository;

        public RegisterHandler(IIdentityService identityService, IUserRepository identitiesRepository)
        {
            _identityService = identityService;
            _identitiesRepository = identitiesRepository;
        }

        public async Task<Unit> Handle(Register request, CancellationToken cancellationToken)
        {
            if (await _identitiesRepository.IsSameUserAlreadyExist(null, request.Username, request.Email,
                    cancellationToken))
                throw new ApiException(HttpStatusCode.BadRequest, 
                    ErrorCodes.UserAlreadyExist, "User with provided username already exist.");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = Roles.User,
                Phone = request.Phone
            };

            _identityService.HashPassword(ref user, request.Password);

            _identitiesRepository.Create(user);
            await _identitiesRepository.SaveChanges(cancellationToken);

            return Unit.Value;
        }
    }
}
