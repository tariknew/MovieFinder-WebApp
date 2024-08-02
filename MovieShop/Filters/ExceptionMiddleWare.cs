using MovieShop.Database;
using MovieShop.Filters;
using Newtonsoft.Json;
using System.Net;
public class ExceptionMiddleWare
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleWare(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context, MWSContext dbContext)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Handle the exception and generate a response
            await HandleExceptionAsync(context, ex);
            LogExceptionToDatabase(ex, dbContext);
        }
    }
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Generate an error response based on the exception
        var response = exception.Message;
        var payload = JsonConvert.SerializeObject(response);
        context.Response.ContentType = "application/json";

        if (exception is UserException userException)
            context.Response.StatusCode = userException.Status;
        else
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(payload);
    }

    private void LogExceptionToDatabase(Exception exception, MWSContext context)
    {
        var newException = new ExceptionLog
        {
            CreationDate = DateTime.Now,
            Message = exception.Message,
            StackTrace = exception.StackTrace
        };

        if (exception is UserException userException)
            newException.StatusCode = userException.Status;
        else
            newException.StatusCode = (int)HttpStatusCode.InternalServerError;

        context.ExceptionLogs.Add(newException);
        context.SaveChanges();
    }
}