using ECommerce.Web.CustomMiddlewares;
using PresentationLayer.CustomMiddlewares;

namespace ECommerce.Web.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApplicationMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<NotFoundEndpointMiddleware>();

            return app;
        }
    }
}
