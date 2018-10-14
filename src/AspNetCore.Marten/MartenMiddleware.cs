using System.Threading;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Http;
using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.AspNetCore.Marten
{
    /// <summary>
    /// Middleare that ensures that changes are saved after each request
    /// </summary>
    public class MartenMiddleware
    {
        private readonly RequestDelegate _next;

        /// <param name="next"></param>
        public MartenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <param name="httpContext"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext, IDocumentSession session)
        {
            await _next(httpContext);
            await session.SaveChangesAsync(httpContext.RequestAborted);
        }
    }
}
