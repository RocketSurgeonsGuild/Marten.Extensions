using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Rocket.Surgery.Extensions.Marten.AspNetCore
{
    /// <summary>
    /// Startup filter to ensure that save changes is called on each request
    /// Implements the <see cref="IStartupFilter" />
    /// </summary>
    /// <seealso cref="IStartupFilter" />
    [UsedImplicitly]
    public class MartenStartupFilter : IStartupFilter
    {
        /// <summary>
        /// Configures the specified next.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <returns>Action{IApplicationBuilder}.</returns>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) => builder =>
        {
            builder.UseMiddleware<MartenMiddleware>();
            next(builder);
        };
    }
}