using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PresentationLayer.CustomMiddlewares
{
    public class NotFoundEndpointMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundEndpointMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            // Only run if no endpoint matched
            if (context.Response.StatusCode == StatusCodes.Status404NotFound &&
                !context.Response.HasStarted)
            {
                var response = new
                {
                    Status = 404,
                    Message = "Endpoint not found"
                };

                var json = JsonSerializer.Serialize(response);

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
        }
    }
}
