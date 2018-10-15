using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Rocket.Surgery.Extensions.Marten.AspNetCore
{
    /// <summary>
    /// Startup filter to ensure that save changes is called on each request
    /// </summary>
    public class MartenStartupFilter : IStartupFilter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
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
