using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthyTasty.Infrastructure.Enums;

namespace HealthyTasty.Dto
{
    internal class ImageDto : BaseDto
    {
        public string AwsImageId { get; set; }
        public ImageType ImageType { get; set; }
    }
}
