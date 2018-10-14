using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.AspNetCore.Marten;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;

[assembly: Convention(typeof(MartenServicesConvention))]

namespace Rocket.Surgery.AspNetCore.Marten
{
    public class MartenServicesConvention : IServiceConvention
    {
        public void Register(IServiceConventionContext context)
        {
            context.WithMarten().AddStartupFilter();
        }
    }
}
