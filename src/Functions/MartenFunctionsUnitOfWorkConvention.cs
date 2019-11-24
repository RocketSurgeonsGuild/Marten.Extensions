using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Marten.Functions;
using Rocket.Surgery.Extensions.WebJobs;

[assembly: Convention(typeof(MartenFunctionsUnitOfWorkConvention))]

namespace Rocket.Surgery.Extensions.Marten.Functions
{
    /// <summary>
    /// MartenFunctionsUnitOfWorkConvention.
    /// Implements the <see cref="IWebJobsConvention" />
    /// </summary>
    /// <seealso cref="IWebJobsConvention" />
    public class MartenFunctionsUnitOfWorkConvention : IWebJobsConvention
    {
        private readonly MartenOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenFunctionsUnitOfWorkConvention" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MartenFunctionsUnitOfWorkConvention(MartenOptions? options = null)
            => _options = options ?? new MartenOptions();

        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(IWebJobsConventionContext context)
        {
            if (_options.AutomaticUnitOfWork)
            {
#pragma warning disable 618
                context.Services.TryAddEnumerable(ServiceDescriptor.Scoped<IFunctionFilter, MartenFunctionFilter>());
#pragma warning restore 618
            }
        }
    }
}