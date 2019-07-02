using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rocket.Surgery.Extensions.Marten.AspNetCore;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;

[assembly: Convention(typeof(MartenMiddlewareUnitOfWorkConvention))]

namespace Rocket.Surgery.Extensions.Marten.AspNetCore
{
    /// <summary>
    /// MartenMiddlewareUnitOfWorkConvention.
    /// Implements the <see cref="Rocket.Surgery.Extensions.DependencyInjection.IServiceConvention" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Extensions.DependencyInjection.IServiceConvention" />
    public class MartenMiddlewareUnitOfWorkConvention : IServiceConvention
    {
        private readonly MartenOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenMiddlewareUnitOfWorkConvention"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MartenMiddlewareUnitOfWorkConvention(MartenOptions options = null)
        {
            _options = options ?? new MartenOptions();
        }
        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(IServiceConventionContext context)
        {
            if (_options.AutomaticUnitOfWork)
            {
                context.Services.TryAddEnumerable(ServiceDescriptor.Transient<IStartupFilter, MartenStartupFilter>());
            }
        }
    }
}
