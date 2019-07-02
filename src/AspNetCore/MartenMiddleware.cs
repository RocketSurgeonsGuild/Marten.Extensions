using System.Threading;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Http;
using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.Extensions.Marten.AspNetCore
{
    /// <summary>
    /// Middleare that ensures that changes are saved after each request
    /// </summary>
    public class MartenMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public MartenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the specified HTTP context.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="session">The session.</param>
        /// <returns>Task.</returns>
        public async Task Invoke(HttpContext httpContext, IDocumentSession session)
        {
            await _next(httpContext);
            await session.SaveChangesAsync(httpContext.RequestAborted);
        }
    }
}
