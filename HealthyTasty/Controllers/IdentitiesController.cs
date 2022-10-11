using HealthyTasty.Commands.Identities;
using HealthyTasty.Domain.Tables;
using HealthyTasty.Dto;
using HealthyTasty.Infrastructure.Jwt;
using HealthyTasty.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HealthyTasty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IdentitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(JsonWebToken))]
        public async Task<ActionResult<JsonWebToken>> Login(Login command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("register")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> Register(Register command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(JsonWebToken))]
        public async Task<ActionResult<JsonWebToken>> RefreshToken(RefreshAccessToken command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
