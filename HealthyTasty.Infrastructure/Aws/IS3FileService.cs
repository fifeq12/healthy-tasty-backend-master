using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace HealthyTasty.Infrastructure.Aws
{
    public class S3FileService : IS3FileService
    {
        private readonly IAmazonS3 _amazonS3;

        public S3FileService(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3;
        }

        public async Task UploadFileToS3(string key, Stream input)
        {
            await _amazonS3.PutObjectAsync(new PutObjectRequest
            {
                CannedACL = S3CannedACL.PublicRead,
                InputStream = input,
                BucketName = "healthy-tasty",
                Key = key
            });
        }
    }

    public interface IS3FileService
    {
        Task UploadFileToS3(string key, Stream input);
    }
}
