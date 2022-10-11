using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;

namespace HealthyTasty.Infrastructure.Aws
{
    public static class AwsExtension
    {
        public const string SectionName = "aws";

        public static void AddAws(this IServiceCollection serviceCollection, AwsOptions options)
        {
            var awsOptions = new AWSOptions()
            {
                Profile = options.Profile
            };

            serviceCollection.AddAWSService<IAmazonS3>(awsOptions, ServiceLifetime.Scoped);
            serviceCollection.AddScoped<IS3FileService, S3FileService>();
        }
    }
}
