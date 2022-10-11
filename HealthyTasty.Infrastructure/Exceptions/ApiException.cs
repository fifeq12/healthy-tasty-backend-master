using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HealthyTasty.Infrastructure.Exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public ErrorCodes ErrorCode { get; set; }

        public ApiException(HttpStatusCode httpStatusCode, ErrorCodes errorCode, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
            ErrorCode = errorCode;
        }
    }
}
