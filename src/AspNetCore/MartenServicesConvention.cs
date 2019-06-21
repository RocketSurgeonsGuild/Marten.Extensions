using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Marten.AspNetCore;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;

namespace Rocket.Surgery.Extensions.Marten.AspNetCore
{
    public class MartenMiddlewareUnitOfWorkConvention : IServiceConvention
    {
        public void Register(IServiceConventionContext context)
        {
            context.WithMarten().AddStartupFilter();
        }
    }
}
