using DomainLayer.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace ECommerce.Web.CustomMiddlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode status;
            object response;

            if (ex is ProductNotFoundException)
                status = HttpStatusCode.NotFound;
            else if (ex is UserNotFoundException)
                status = HttpStatusCode.NotFound;
            else if (ex is UnauthorizedException)
                status = HttpStatusCode.Unauthorized;
            else if (ex is UserAlreadyExistsException)
                status = HttpStatusCode.Conflict;
            else if (ex is IdentityOperationException identityEx)
            {
                status = HttpStatusCode.BadRequest;

                var errors = new List<string>();
                if (identityEx.Errors != null)
                {
                    foreach (var error in identityEx.Errors)
                    {
                        errors.Add(error.Description);
                    }
                }

                response = new
                {
                    Code = (int)status,
                    Message = identityEx.Message,
                    Errors = errors
                };

                var jsonIdentity = JsonSerializer.Serialize(response);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)status;
                return context.Response.WriteAsync(jsonIdentity);
            }
            else
                status = HttpStatusCode.InternalServerError;

            response = new
            {
                Code = (int)status,
                Message = ex.Message
            };

            var json = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(json);
        }
    }
}
