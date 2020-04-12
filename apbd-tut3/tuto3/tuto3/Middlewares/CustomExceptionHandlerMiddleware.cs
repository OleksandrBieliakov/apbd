using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tuto3.Models;

namespace tuto3.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exc)
            {
                await HandleException(context, exc);
            }
        }

        private Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // It's never thrown yet
            if (ex is CustomExceptionHandlerMiddleware)
            {
                return context.Response.WriteAsync(new ErrorDetails
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "My custom error handler has cought my custom exception"
                }.ToString());
            }

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "My custom error handler has cought some exception"
            }.ToString());
        }

    }
}
