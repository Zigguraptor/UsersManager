namespace UsersManager.Application.Common.Exceptions;

public class HttpException : Exception
{
    public int StatusCode { get; }
    public string? ResponseMessage { get; }

    public HttpException(int statusCode, string? message) : base(message)
    {
        StatusCode = statusCode;
        ResponseMessage = message;
    }

    public HttpException(int statusCode, string? message, Exception? innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
        ResponseMessage = message;
    }
}
