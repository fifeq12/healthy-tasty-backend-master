using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HealthyTasty.Infrastructure.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e, _logger);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            string? result = null;
            
            switch (exception)
            {
                case ApiException ae:
                    response.StatusCode = (int)ae.HttpStatusCode;
                    logger.LogError(ae, ae.ErrorCode.ToString());
                    result = JsonSerializer.Serialize(new 
                    { 
                        ae.ErrorCode, 
                        ErrorCodeMessage = ae.ErrorCode.ToString(), 
                        ae.Message
                    });
                    break;
                case { } ex:
                    response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    logger.LogError(ex, "Unhandled exception.");
                    result = JsonSerializer.Serialize(new
                    {
                        ErrorCode = ErrorCodes.UnhandledException,
                        ErrorCodeMessage = ErrorCodes.UnhandledException.ToString(), 
                        Message = "Unhandled exception."
                    });
                    break;
            }

            await context.Response.WriteAsync(result ?? "{}");
        }
    }
}
