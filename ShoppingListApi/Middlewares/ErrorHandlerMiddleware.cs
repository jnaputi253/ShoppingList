using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ShoppingListApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate requestDelegate, ILogger<ErrorHandlerMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate.Invoke(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
            }
        }
    }
}
