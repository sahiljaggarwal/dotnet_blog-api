using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BlogPortal.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var httpContext = context.HttpContext;

            var response = new
            {
                statusCode = 500,
                success = false,
                timestamp = DateTime.UtcNow.ToString("o"),
                path = httpContext.Request.Path,
                message = "Something went wrong"
            };

            if (exception is ValidationException validationEx)
            {
                response = new
                {
                    statusCode = (int)HttpStatusCode.BadRequest,
                    success = false,
                    timestamp = DateTime.UtcNow.ToString("o"),
                    path = httpContext.Request.Path,
                    message = validationEx.Message
                };
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exception is DbUpdateException dbEx)
            {
                response = new
                {
                    statusCode = 500,
                    success = false,
                    timestamp = DateTime.UtcNow.ToString("o"),
                    path = httpContext.Request.Path,
                    message = $"Database Error: {dbEx.InnerException?.Message ?? dbEx.Message}"
                };
                context.HttpContext.Response.StatusCode = 500;
            }
            else if (exception is HttpRequestException httpEx)
            {
                response = new
                {
                    statusCode = 503,
                    success = false,
                    timestamp = DateTime.UtcNow.ToString("o"),
                    path = httpContext.Request.Path,
                    message = $"HTTP Error: {httpEx.Message}"
                };
                context.HttpContext.Response.StatusCode = 503;
            }
            else
            {
                response = new
                {
                    statusCode = 500,
                    success = false,
                    timestamp = DateTime.UtcNow.ToString("o"),
                    path = httpContext.Request.Path,
                    message = exception.Message
                };
                context.HttpContext.Response.StatusCode = 500;
            }

            context.Result = new JsonResult(response);
            context.ExceptionHandled = true;
        }
    }
}
