using System.Net;
using HealthyTasty.Commands.Images;
using HealthyTasty.Dto;
using HealthyTasty.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthyTasty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UploadImage(IFormFile file)
        {
            return Ok(await _mediator.Send(new UploadImage() { File = file}));
        }
    }
}
