using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Marten.AspNetCore;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Marten.Functions;

[assembly: Convention(typeof(MartenServicesConvention))]

namespace Rocket.Surgery.Extensions.Marten.Functions
{
    public class MartenServicesConvention : IServiceConvention
    {
        public void Register(IServiceConventionContext context)
        {
            context.WithMarten().AddFunctionFilters();
        }
    }
}
