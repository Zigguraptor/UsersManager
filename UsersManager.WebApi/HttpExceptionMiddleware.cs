using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using UsersManager.Application.Common.Exceptions;

namespace UsersManager.WebApi;

public class HttpExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public HttpExceptionMiddleware(RequestDelegate next) => _next = next;

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
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "text/plain";
            if (ex.ResponseMessage != null)
                await context.Response.WriteAsync(ex.ResponseMessage);
        }
    }
}
