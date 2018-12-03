using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GildedRose.Api.Middleware
{
    public class SessionInitializerMiddleware
    {
        private readonly RequestDelegate next;

        public SessionInitializerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await httpContext.Session.LoadAsync();

            var hello = httpContext.Session.GetString("Hello");

            if (hello == null)
            {
                httpContext.Session.SetString("Hello", "Hello");
            }

            await this.next(httpContext);
            await httpContext.Session.CommitAsync();
        }
    }
}
