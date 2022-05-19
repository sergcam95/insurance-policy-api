using System.Net;
using System.Text;
using System.Text.Json;
using InsurancePolicyService.API.Models;
using InsurancePolicyService.Application.Common.Exceptions;

namespace InsurancePolicyService.API.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            var errorResponse = new ErrorResponse
            {
                ErrorMessage = e.Message
            };
            
            _logger.LogError(e.Message);
            var errorResponseBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(errorResponse, new
                JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
            
            context.Response.StatusCode = (int)GetStatusCodeBasedOnException(e);
            context.Response.Headers.ContentType = "application/json";
            await context.Response.Body.WriteAsync(errorResponseBytes, 0, errorResponseBytes.Length);
        }
    }
    
    private HttpStatusCode GetStatusCodeBasedOnException(Exception e)
    {
        switch (e)
        {
            case RequestValidationException:
                return HttpStatusCode.BadRequest;
            default:
                return HttpStatusCode.InternalServerError;
        }
    }
}