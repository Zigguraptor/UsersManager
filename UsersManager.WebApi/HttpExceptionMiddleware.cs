using UsersManager.Application.Common.Exceptions;

namespace UsersManager.WebApi;

public class HttpExceptionMiddleware
{
    private readonly ILogger<HttpExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public HttpExceptionMiddleware(ILogger<HttpExceptionMiddleware> logger, RequestDelegate next)
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
        catch (Exception e)
        {
            if (e is not HttpException ex) throw;

            context.Response.Clear();
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "text/plain";
            
            _logger.LogInformation("Handled http exception to ip:{ipAddress}; status code: {code}; {message}",
                context.Connection.RemoteIpAddress?.ToString(), ex.StatusCode, ex.ResponseMessage);
            
            if (ex.ResponseMessage != null)
                await context.Response.WriteAsync(ex.ResponseMessage);
        }
    }
}
