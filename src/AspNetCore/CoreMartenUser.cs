using Microsoft.AspNetCore.Http;
using Rocket.Surgery.Extensions.Marten;
using Microsoft.Extensions.Options;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    class CoreMartenUser : IMartenUser
    {
        private readonly MartenOptions _options;
        private readonly IHttpContextAccessor _accessor;
        private string _id;

        public CoreMartenUser(IOptions<MartenOptions> options, IHttpContextAccessor accessor)
        {
            _options = options.Value;
            _accessor = accessor;
        }

        public object Id
        {
            get
            {
                if (string.IsNullOrEmpty(_id))
                {
                    if (_accessor.HttpContext.User.Identity.IsAuthenticated)
                    {
                        _id = _accessor.HttpContext.User.Claims
                          .Where(_options.IsIdLikeClaim)
                          .FirstOrDefault()?.Value;
                    }
                    else
                    {
                        _id = "**anonymous**";
                    }
                }
                return _id;
            }
        }
    }
}
