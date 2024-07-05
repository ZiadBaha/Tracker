using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Account.Apis.Errors
{
    public class ExeptionMiddleWares
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExeptionMiddleWares> logger;
        private readonly IHostEnvironment env;

        public ExeptionMiddleWares(RequestDelegate next, ILogger<ExeptionMiddleWares> logger, IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }

        // Middleware logic to handle exceptions
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Invoke the next middleware in the pipeline
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex, ex.Message);

                // Set response content type and status code
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Create ApiResponse based on environment
                var response = env.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                                                   : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

                // Configure JSON serialization options
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                // Serialize ApiResponse to JSON
                var jsonResponse = JsonSerializer.Serialize(response, options);

                // Write JSON response to the HTTP response
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }

}
