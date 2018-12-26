using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System;

namespace Rocket.Surgery.Extensions.Marten
{
    public static class ClaimsExtensions
    {
        private static readonly string[] IdKeys = { "user_id", "sub" };
        public static string GetIdFromClaims(this IEnumerable<Claim> claims)
        {
            return IdKeys
                .Select(key => claims.FirstOrDefault(c => c.Type == key))
                .FirstOrDefault()?.Value;
        }

        private static bool IsIdLikeClaim(Claim claim)
        {
            if (claim.Type == ClaimTypes.NameIdentifier)
            {
                return true;
            }

            if ((claim.Type.StartsWith("http://") || claim.Type.StartsWith("https://"))
            && (claim.Type.EndsWith("/sub") || claim.Type.EndsWith("/id") || claim.Type.EndsWith("/user_id")))
            {
                return true;
            }

            return IdKeys.Any(z => claim.Type == z);
        }
    }
}
