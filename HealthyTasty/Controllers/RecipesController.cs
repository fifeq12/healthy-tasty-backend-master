using System.Net;
using AutoMapper;
using HealthyTasty.Domain;
using HealthyTasty.Domain.Tables;
using HealthyTasty.Dto;
using HealthyTasty.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyTasty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "User")]
    public class RecipesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecipesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<BaseRecipeDto>))]
        public async Task<ActionResult<IEnumerable<BaseRecipeDto>>> GetAllRecipes([FromQuery] GetAllRecipes recipes)
        {
            return Ok(await _mediator.Send(recipes));
        }
    }
}
