using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rocket.Surgery.Extensions.Marten.AspNetCore;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Marten.Functions;
using Rocket.Surgery.Extensions.WebJobs;

namespace Rocket.Surgery.Extensions.Marten.Functions
{
    /// <summary>
    ///  MartenFunctionsUnitOfWorkConvention.
    /// Implements the <see cref="Rocket.Surgery.Extensions.WebJobs.IWebJobsConvention" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Extensions.WebJobs.IWebJobsConvention" />
    public class MartenFunctionsUnitOfWorkConvention : IWebJobsConvention
    {
        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(IWebJobsConventionContext context)
        {
#pragma warning disable 618
            context.Services.TryAddEnumerable(ServiceDescriptor.Scoped<IFunctionFilter, MartenFunctionFilter>());
#pragma warning restore 618
        }
    }
}
