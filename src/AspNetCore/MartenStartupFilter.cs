using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Rocket.Surgery.Extensions.Marten.AspNetCore
{
    /// <summary>
    /// Startup filter to ensure that save changes is called on each request
    /// Implements the <see cref="Microsoft.AspNetCore.Hosting.IStartupFilter" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Hosting.IStartupFilter" />
    public class MartenStartupFilter : IStartupFilter
    {
        /// <summary>
        /// Configures the specified next.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <returns>Action&lt;IApplicationBuilder&gt;.</returns>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                builder.UseMiddleware<MartenMiddleware>();
                next(builder);
            };
        }
    }
}
