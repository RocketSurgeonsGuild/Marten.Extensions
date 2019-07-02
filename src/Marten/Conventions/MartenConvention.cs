using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Marten.Conventions;

[assembly:Convention(typeof(MartenConvention))]

namespace Rocket.Surgery.Extensions.Marten.Conventions
{
    /// <summary>
    ///  MartenConvention.
    /// Implements the <see cref="Rocket.Surgery.Extensions.DependencyInjection.IServiceConvention" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Extensions.DependencyInjection.IServiceConvention" />
    public class MartenConvention : IServiceConvention
    {
        private readonly MartenOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="MartenConvention"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MartenConvention(MartenOptions options = null)
        {
            _options = options ?? new MartenOptions();
        }

        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(IServiceConventionContext context)
        {
            context.WithMarten();

            var connectionString =
                !string.IsNullOrEmpty(_options.ConnectionString) ? _options.ConnectionString :
                context.Configuration.GetValue<string>("PostgresSql:ConnectionString", null)
                ?? context.Configuration.GetValue<string>("Postgres:ConnectionString", null)
                ?? context.Configuration.GetValue<string>("Marten:ConnectionString", null);

            if (_options.UseSession)
            {
                context.Services.TryAddScoped(c => c.GetRequiredService<IDocumentStore>().OpenSession(_options.SessionTracking));
            }

            if (!string.IsNullOrEmpty(connectionString))
            {
                context.Services.Configure<StoreOptions>(options => { options.Connection(connectionString); });
            }
        }
    }
}
