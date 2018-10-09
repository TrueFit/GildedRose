using guilded.rose.api.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;

namespace guilded.rose.api.Infrastructure.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}