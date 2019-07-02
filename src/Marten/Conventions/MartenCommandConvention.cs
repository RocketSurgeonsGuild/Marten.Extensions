using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.CommandLine;
using Rocket.Surgery.Extensions.Marten.Conventions;

[assembly: Convention(typeof(MartenCommandConvention))]

namespace Rocket.Surgery.Extensions.Marten.Conventions
{
    /// <summary>
    /// MartenCommandConvention.
    /// Implements the <see cref="Rocket.Surgery.Extensions.CommandLine.ICommandLineConvention" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Extensions.CommandLine.ICommandLineConvention" />
    public class MartenCommandConvention : ICommandLineConvention
    {
        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(ICommandLineConventionContext context)
        {
            context.AddCommand<MartenCommand>();
        }
    }
}
