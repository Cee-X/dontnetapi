using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NZWalks.Middleware
{
   public class ExceptionHandler
   {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandler> _logger;
    public ExceptionHandler(ILogger<ExceptionHandler> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var errorId = Guid.NewGuid();
            //Log the exception
            _logger.LogError(ex,$"{errorId} : {ex.Message}");

            //return a custom error response

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var error = new 
            {
                Id = errorId,
                ErrorMessage = "Something went wrong."
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(error));
        }
    }
   } 
}