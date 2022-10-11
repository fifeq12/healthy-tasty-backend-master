using HealthyTasty.Infrastructure.Aws;
using MediatR;

namespace HealthyTasty.Commands.Images
{
    public class UploadImage : IRequest
    {
        public IFormFile File { get; set; }
    }

    public class UploadImageHandler : IRequestHandler<UploadImage>
    {
        private readonly IS3FileService _s3FileService;

        public UploadImageHandler(IS3FileService s3FileService)
        {
            _s3FileService = s3FileService;
        }

        public async Task<Unit> Handle(UploadImage request, CancellationToken cancellationToken)
        {
            await _s3FileService.UploadFileToS3(request.File.FileName, request.File.OpenReadStream());

            return Unit.Value;
        }
    }
}
