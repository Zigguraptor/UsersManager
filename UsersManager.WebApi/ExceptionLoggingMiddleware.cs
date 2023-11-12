namespace UsersManager.WebApi;

public class ExceptionLoggingMiddleware
{
    private readonly ILogger<ExceptionLoggingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionLoggingMiddleware(ILogger<ExceptionLoggingMiddleware> logger, RequestDelegate next)
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
            _logger.LogError("Unhandled exception remote ip:{ip};\n {exception};",
                context.Connection.RemoteIpAddress?.ToString(), e.ToString());
            
            throw;
        }
    }
}
