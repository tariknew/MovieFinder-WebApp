using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;

namespace MovieShop.Filters
{
    public class ErrorFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // Handle the exception and generate a response
            var response = context.Exception.Message;
            var payload = JsonConvert.SerializeObject(response);
            context.Result = new ContentResult
            {
                Content = payload,
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
            context.ExceptionHandled = true;
        }
    }
}

