using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ToDoAppWebAPI
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
           
            context.Response.ContentType = "application/json";
            if (exception is UnauthorizedAccessException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var errorResponse = new
                {
                    Message = "Unauthorized access.",
                    ExceptionMessage = exception.Message
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var errorResponse = new
                {
                    Message = "An error occurred while processing your request.",
                    ExceptionMessage = exception.Message,
                    StackTrace = exception.StackTrace
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
        }
    }

}
