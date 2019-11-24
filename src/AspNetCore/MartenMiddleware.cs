using System.Threading.Tasks;
using JetBrains.Annotations;
using Marten;
using Microsoft.AspNetCore.Http;

namespace Rocket.Surgery.Extensions.Marten.AspNetCore
{
    /// <summary>
    /// Middleware that ensures that changes are saved after each request
    /// </summary>
    [UsedImplicitly]
    internal class MartenMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public MartenMiddleware(RequestDelegate next) => _next = next;

        /// <summary>
        /// Invokes the specified HTTP context.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="session">The session.</param>
        /// <returns>Task.</returns>
        public async Task Invoke(HttpContext httpContext, IDocumentSession session)
        {
            await _next(httpContext).ConfigureAwait(false);
            await session.SaveChangesAsync(httpContext.RequestAborted).ConfigureAwait(false);
        }
    }
}