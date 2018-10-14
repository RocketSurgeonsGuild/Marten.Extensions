using Microsoft.AspNetCore.Hosting;
using Rocket.Surgery.AspNetCore.Marten;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.CommandLine;
using Microsoft.Extensions.DependencyInjection;

[assembly: Convention(typeof(MartenCommandConvention))]

namespace Rocket.Surgery.AspNetCore.Marten
{
    public class MartenCommandConvention : ICommandLineConvention
    {
        public void Register(ICommandLineConventionContext context)
        {
            context.AddCommand<MartenCommand>("marten");
        }
    }
}
