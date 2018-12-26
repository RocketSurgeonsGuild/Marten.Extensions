using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System;

namespace Rocket.Surgery.Extensions.Marten
{
    public static class ClaimsExtensions
    {
        private static readonly string[] IdKeys = { "user_id", "sub" };
        public static string GetIdFromClaims(this IEnumerable<Claim> claims, MartenOptions options)
        {
            return claims
                .Where(options.IsIdLikeClaim)
                .FirstOrDefault()?.Value;
        }
    }
}
