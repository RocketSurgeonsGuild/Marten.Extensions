using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Marten.AspNetCore;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;

[assembly: Convention(typeof(MartenServicesConvention))]

namespace Rocket.Surgery.Extensions.Marten.AspNetCore
{
    public class MartenServicesConvention : IServiceConvention
    {
        public void Register(IServiceConventionContext context)
        {
            context.WithMarten().AddStartupFilter();
        }
    }
}
