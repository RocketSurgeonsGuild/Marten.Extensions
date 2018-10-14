using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Azure.Functions.Marten;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;

[assembly: Convention(typeof(MartenServicesConvention))]

namespace Rocket.Surgery.Azure.Functions.Marten
{
    public class MartenServicesConvention : IServiceConvention
    {
        public void Register(IServiceConventionContext context)
        {
            context.WithMarten().AddSaveChangesFilter();
        }
    }
}
