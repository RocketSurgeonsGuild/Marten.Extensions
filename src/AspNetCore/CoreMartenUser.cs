using Microsoft.AspNetCore.Http;
using Rocket.Surgery.Extensions.Marten;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    class CoreMartenUser : IMartenUser
    {
        public CoreMartenUser(IOptions<MartenOptions> options, IHttpContextAccessor accessor)
        {
            Id = accessor.HttpContext.User.Claims.GetIdFromClaims(options.Value);
        }

        public object Id { get; }
    }
}
